import React from 'react';
import { Button, Modal } from 'reactstrap';
import Image from 'react-bootstrap/Image';
import moment from 'moment';
import axios from 'axios';
import { getCookie, isUserSignedIn } from 'components/Tools/Tools';
import './Event.css';
import './EventMobile.css';
import { confirmAlert } from 'react-confirm-alert';
import 'react-confirm-alert/src/react-confirm-alert.css';
import { Chat } from '../Chat/Chat';
import {
  FacebookShareButton,
  FacebookIcon,
  TwitterShareButton,
  TwitterIcon,
  EmailShareButton,
  EmailIcon,
  WhatsappShareButton,
  WhatsappIcon,
} from 'react-share';

class Event extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      event: [],
      boardGame: [],
      players: [],
      signedIn: isUserSignedIn(),
      modal: false,
    };
    this.toggle = this.toggle.bind(this);
  }
  toggle() {
    this.setState(prevState => ({
      modal: !prevState.modal,
    }));
  }

  componentDidMount() {
    axios
      .get(
        process.env.REACT_APP_BACK +
          '/api/GameSessions/' +
          this.props.match.params.id,
        {
          headers: {
            Authorization: 'Bearer ' + getCookie('token'),
          },
        },
      )
      .then(res => {
        const data = res.data;
        this.setState({ boardGame: data.boardGamesEvent[0] });
        this.setState({ players: data.players });
        this.setState({ event: data });
      });
    console.log(this.state.boardGame);
  }

  joinEvent = () => {
    confirmAlert({
      title: 'Musisz potwierdzić',
      message: 'Jesteś pewny dołączenia do wydarzenia?',
      buttons: [
        {
          color: 'success',
          label: 'Tak',
          onClick: () =>
            axios
              .post(
                process.env.REACT_APP_BACK +
                  '/api/GameSessions/JoinGameSession/' +
                  this.props.match.params.id,
                {},
                {
                  headers: {
                    Authorization: 'Bearer ' + getCookie('token'),
                  },
                },
              )
              .then(res => {
                window.location.reload();
              }),
        },
        {
          color: 'danger',
          label: 'Nie',
        },
      ],
    });
  };

  leaveEvent = () => {
    confirmAlert({
      title: 'Musisz potwierdzić',
      message: 'Jesteś pewny, że chcesz opuścić wydarzenie?',
      buttons: [
        {
          color: 'success',
          label: 'Tak',
          onClick: () =>
            axios
              .post(
                process.env.REACT_APP_BACK +
                  '/api/GameSessions/LeaveGameSession/' +
                  this.props.match.params.id,
                {},
                {
                  headers: {
                    Authorization: 'Bearer ' + getCookie('token'),
                  },
                },
              )
              .then(res => {
                window.location.reload();
              }),
        },
        {
          color: 'danger',
          label: 'Nie',
        },
      ],
    });
  };

  kickPlayer(playerId) {
    confirmAlert({
      title: 'Musisz potwierdzić',
      message: 'Jesteś pewny, że chcesz wyrzucić tego gracza?',
      buttons: [
        {
          color: 'success',
          label: 'Tak',
          onClick: () =>
            axios
              .post(
                process.env.REACT_APP_BACK +
                  '/api/GameSessions/DeletePlayerFromGameSessionByAdmin',
                {
                  userID: playerId,
                  gameSessionID: this.props.match.params.id,
                },
                {
                  headers: {
                    Authorization: 'Bearer ' + getCookie('token'),
                  },
                },
              )
              .then(res => {
                window.location.reload();
              }),
        },
        {
          color: 'danger',
          label: 'Nie',
        },
      ],
    });
  }

  deleteEvent = () => {
    confirmAlert({
      title: 'Musisz potwierdzić',
      message: 'Jesteś pewny, że chcesz usunąć to wydarzenie?',
      buttons: [
        {
          color: 'success',
          label: 'Tak',
          onClick: () =>
            axios
              .delete(
                process.env.REACT_APP_BACK +
                  '/api/GameSessions/' +
                  this.props.match.params.id,
                {
                  headers: {
                    Authorization: 'Bearer ' + getCookie('token'),
                  },
                },
              )
              .then(res => {
                window.location = `/events`;
              }),
        },
        {
          color: 'danger',
          label: 'Nie',
        },
      ],
    });
  };

  render() {
    return (
      <div id="EventMain">
        {/* start Main */}
        <div id="EventEvent">
          {/* START Event */}
          <div id="EventHeader">{this.state.event.name}</div>
          <div id="EventGameImageContainer">
            <div id="EventGameImage">
              {/* start GameImage */}

              <Image
                fluid
                src={this.state.boardGame.boardGameAvatar}
                alt="Generic placeholder"
                onClick={this.toggle}
                // roundedCircle
                width="130px"
                height="130px"
              >
                {this.props.buttonLabel}
              </Image>
              {/* Start Popup game description */}
              <Modal
                isOpen={this.state.modal}
                toggle={this.toggle}
                className={this.props.className}
              >
                <div id="event_popup">
                  <div id="modal-header">
                    <h3 id="text-center">Opis gry</h3>
                  </div>
                  <div id="event_popup_image">
                    <Image
                      fluid
                      width="120px"
                      height="120px"
                      src={this.state.boardGame.boardGameAvatar}
                      alt="Generic placeholder"
                      rounded
                      onClick={this.toggle}
                    />
                  </div>
                  <div id="event_popup_col_left">
                    Nazwa gry <br />
                    Liczba Graczy <br />
                    Wiek <br />
                    Czas gry <br />
                  </div>
                  <div id="event_popup_col_right">
                    {this.state.boardGame.name}
                    <br />
                    {this.state.boardGame.playersMin} -{' '}
                    {this.state.boardGame.playersMax}
                    <br />
                    {this.state.boardGame.age}
                    <br />
                    {this.state.boardGame.gameTimeMin} - {''}
                    {this.state.boardGame.gameTimeMax}
                    <br />
                    {/* {this.state.boardGame.description} */}
                    <br />
                  </div>
                </div>
              </Modal>
              {/* End popup game description */}
              {/* End GameImage */}
            </div>
            <div id="EventGameImageLabel">
              {this.state.boardGame.name}
            </div>
          </div>
          <div id="EventDescription">
            <div id="EventDescriptionLeft">
              <div id="EventDescriptionCity">
                <div id="EventDescriptionCityLabel">Miasto:</div>
                <div id="EventDescriptionCityDescription">
                  {this.state.event.city}
                </div>
              </div>
              <div id="EventDescriptionAddress">
                <div id="EventDescriptionAddressLabel">Adres:</div>
                <div id="EventDescriptionAddressDescription">
                  {this.state.event.address}
                </div>
              </div>
              <div id="EventDescriptionDate">
                <div id="EventDescriptionDateLabel">Kiedy:</div>
                <div id="EventDescriptionDateDescription">
                  {moment(this.state.event.timeStart)
                    .utcOffset(2)
                    .format('DD-MM-YYYY, HH:mm')}
                </div>
              </div>
              <div id="EventShareMessage">Udostępnij znajomym:</div>
              {/* <div id="EventDescriptionGamename">
                <div id="EventDescriptionGamenameLabel">Gra:</div>
                <div id="EventDescriptionGamenameDescription">
                  {this.state.boardGame.name}
                </div>
              </div> */}
            </div>
            <div id="EventDescriptionRight">
              <div id="EventDescriptionProperties">
                <div id="EventDescriptionPropertiesLabel">Opis:</div>
                <div id="EventDescriptionPropertiesDescription">
                  {this.state.event.description}
                </div>
              </div>
              <div id="EventShare">
                <div id="EventShareComponent">
                  <FacebookShareButton
                    url={
                      'https://getonboard.pl/' +
                      window.location.pathname
                    }
                  >
                    <FacebookIcon size={50} round={true} />
                  </FacebookShareButton>{' '}
                </div>
                <div id="EventShareComponent">
                  <TwitterShareButton
                    url={
                      'https://getonboard.pl/' +
                      window.location.pathname
                    }
                  >
                    <TwitterIcon size={50} round={true} />
                  </TwitterShareButton>
                </div>
                <div id="EventShareComponent">
                  <EmailShareButton
                    url={
                      'https://getonboard.pl/' +
                      window.location.pathname
                    }
                  >
                    <EmailIcon size={50} round={true} />
                  </EmailShareButton>
                </div>
                <div id="EventShareComponent">
                  <WhatsappShareButton
                    url={
                      'https://getonboard.pl/' +
                      window.location.pathname
                    }
                  >
                    <WhatsappIcon size={50} round={true} />
                  </WhatsappShareButton>
                </div>
              </div>
            </div>

            {/* {this.state.event.slots} */}
            {/* END event description description */}
            {/* END event description */}
          </div>
          {/* <div id="EventAdmin">
            <span id="topSpan"></span>

            <br></br>
            <div id="EventAdminAvatar">
              <Image
                width="120px"
                height="120px"
                src={this.state.event.adminAvatar}
                alt="Generic placeholder"
                roundedCircle
              />
            </div>
            <br></br>

            <br></br>
            {this.state.event.adminID}
 
          </div> */}
        </div>
        <div id="">
          {/* START EventChatAndPlayers */}
          <div id="EventChat">
            <span id="bottomSpan"></span>
            {/* START Chat */}
            <div id="EventChatHeader"></div>
            <div id="">{<Chat event={this.state.event}></Chat>}</div>

            {/* END Chat */}
          </div>
          <div id="EventPlayers">
            <div id="EventPlayersPeople">
              {/* START Players */}
              <h1>GRACZE</h1>
              <ul className="EventUl">
                {this.state.players.map(player => {
                  return (
                    <li className="EventUl" key={player.id}>
                      <div id="EventAvatar">
                        {' '}
                        <Image
                          width="40px"
                          height="40px"
                          src={player.avatarPlayer}
                          alt="Generic placeholder"
                          roundedCircle
                        />
                      </div>
                      <a
                        className="EventUl"
                        href={'/profile/' + player.id}
                      >
                        {player.userName}
                      </a>
                    </li>
                  );
                })}
              </ul>
              <br></br>
              {this.state.event.isAdmin ? (
                <br></br>
              ) : (
                <div id="confirmMessage">
                  {' '}
                  {/* DIV or RIP */}
                  {this.state.event.isCurrentUserInGame ? (
                    <Button color="primary" onClick={this.leaveEvent}>
                      {' '}
                      Opuść
                    </Button>
                  ) : (
                    <Button color="primary" onClick={this.joinEvent}>
                      {' '}
                      Dołącz!
                    </Button>
                  )}
                </div>
              )}
              {this.state.event.isAdmin ? (
                <Button color="danger" onClick={this.deleteEvent}>
                  Usuń wydarzenie
                </Button>
              ) : (
                ''
              )}

              {/* END Players */}
            </div>
            <div id="EventPlayersButton">
              <ul className="EventButton">
                {this.state.players.map(player => {
                  return (
                    <li className="EventButton" key={player.id}>
                      <a className="EventButton" href>
                        {this.state.event.isAdmin &&
                        this.state.event.adminID !== player.id ? (
                          <Button
                            id="EventButton"
                            color="danger"
                            onClick={() => this.kickPlayer(player.id)}
                          >
                            {'X'}
                          </Button>
                        ) : (
                          ''
                        )}
                      </a>
                    </li>
                  );
                })}
              </ul>
            </div>
            {/* End EventPlayers */}
          </div>
          {/* End EventChatAndPlayers */}
        </div>
        {/* END Main */}
      </div>
    );
  }
}

export default Event;
