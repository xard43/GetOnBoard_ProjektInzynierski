import React from 'react';
import * as signalR from '@aspnet/signalr';
import moment from 'moment';
import 'moment/locale/pl';
import { getCookie } from 'components/Tools/Tools';
import { Message } from './Message';
import './Chat.scss';
moment().format('LLL');
moment().locale('pl');

export class Chat extends React.Component {
  constructor(props) {
    super(props);
    this.getHistory = this.getHistory.bind(this);

    this.state = {
      message: '',
      messages: [],
      hubConnection: null,
    };
  }

  componentDidMount = () => {
    const hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(process.env.REACT_APP_BACK + '/chat', {
        accessTokenFactory: () => getCookie('token'),
      })
      .configureLogging(signalR.LogLevel.Information)
      .build();

    this.setState({ hubConnection }, () => {
      this.state.hubConnection
        .start()
        .then(() => console.log('Connection started!'))
        //.then(() => this.getHistory())
        .then(() => setTimeout(() => this.getHistory(), 500))
        .catch(err =>
          console.log('Error while establishing connection'),
        );

      this.state.hubConnection.on(
        'ReceiveMessage',
        receivedMessage => {
          let newMessages = [...this.state.messages, receivedMessage];
          this.setState(
            {
              messages: newMessages,
            },
            this.scrollbarToBottom,
          );
        },
      );

      this.state.hubConnection.on('History', receivedMessage => {
        this.setState({ messages: receivedMessage });
      });
    });
  };

  scrollbarToBottom = () => {
    let objDiv = document.getElementsByClassName('messages-main')[0];
    objDiv.scrollTop = objDiv.scrollHeight;
  };
  getHistory = () => {
    this.state.hubConnection
      .invoke('EnterGame', this.props.event.id)
      .then(() => console.log('EnterGame'))
      .catch(err => console.error(err));
  };

  sendMessage = () => {
    this.state.hubConnection
      .invoke('SendMessage', this.state.message, this.props.event.id)
      .catch(err => console.error(err));

    this.setState({ message: '' });
  };

  render() {
    return (
      //mt- margin top
      <div className="chat-container m-0 p-0">
        <div className="row mt-5 messages-main m-0 p-0">
          <div className="col-md-10 offset-md-1 col-sm-10 offset-sm-1 col-11 messages pt-2 rounded">
            {this.state.messages.map((messageObject, index) => (
              <span style={{ display: 'block' }} key={index}>
                <Message messageObject={messageObject} />
              </span>
            ))}
          </div>
        </div>
        <div className="row messages-box-main p-0 rounded-bottom  m-0 ">
          <div className="col-md-9 col-sm-9 col-9 pr-0 messages-box ">
            <input
              type="text"
              className="form-control"
              value={this.state.message}
              placeholder="Napisz wiadomość"
              onChange={e =>
                this.setState({ message: e.target.value })
              }
            />
          </div>
          <button
            className="btn btn-success col col-3"
            onClick={this.sendMessage}
            disabled={!this.state.message}
          >
            Wyślij
          </button>
        </div>
      </div>
    );
  }
}
