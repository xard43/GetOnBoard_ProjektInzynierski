import React from 'react';
import Button from 'react-bootstrap/Button';
import Form from 'react-bootstrap/Form';
import axios from 'axios';
import { store } from 'react-notifications-component';
import { Redirect } from 'react-router';
class Contact extends React.Component {
  constructor(props) {
    super(props);
    this.handleSubmit = this.handleSubmit.bind(this);
    this.state = {
      email: '',
      message: '',
      toSend: true,
    };
  }

  handleEmailChange = event => {
    this.setState({ email: event.target.value });
  };

  handleMessageChange = event => {
    this.setState({ message: event.target.value });
  };

  handleSubmit(event) {
    event.preventDefault();
    this.setState({ toSend: false });
    var dataToPost = {
      email: this.state.email,
      message: this.state.message,
    };

    axios
      .post(process.env.REACT_APP_BACK + '/contact', dataToPost)
      .then(res => {})
      .then(res => {
        store.addNotification({
          title: 'Wiadomość wysłana!',
          message:
            'Dziękujemy! Dzięki Tobie będziemy mieli co robić w wolnym czasie!',
          type: 'success',
          insert: 'top',
          container: 'bottom-right',
          animationIn: ['animated', 'fadeIn'],
          animationOut: ['animated', 'fadeOut'],
        });
      })
      .catch(error => {
        store.addNotification({
          title: error.type,
          message: error.message,
          type: 'danger',
          insert: 'top',
          container: 'bottom-right',
          animationIn: ['animated', 'fadeIn'],
          animationOut: ['animated', 'fadeOut'],
        });
      });
  }

  render() {
    return (
      <div className="container">
        {this.state.toSend ? (
          <div id="ContactForm" className="bg-white p-5">
            <h1 id="ContactTitle">Skontaktuj się z nami!</h1>
            <form onSubmit={this.handleSubmit}>
              <Form.Group className="ContactInput" controlId="email">
                <Form.Label>
                  Podaj swój mail, abyśmy mogli Ci odpowiedzieć
                </Form.Label>
                <Form.Control
                  type="email"
                  className={'form-control'}
                  value={this.state.email}
                  onChange={this.handleEmailChange}
                  placeholder="użytkownik@domena.com"
                />
              </Form.Group>

              <Form.Group
                className="ContactInput"
                controlId="message"
              >
                <Form.Label>Wiadomość</Form.Label>
                <textarea
                  className="form-control"
                  rows="5"
                  id="comment"
                  value={this.state.message}
                  onChange={this.handleMessageChange}
                  placeholder="Napisz proszę w czym możemy Ci pomóc"
                  maxLength="500"
                />
              </Form.Group>

              <Button
                className="RegisterInput"
                variant="primary"
                type="submit"
                disabled={
                  this.state.email === '' || this.state.message === ''
                }
              >
                Wyślij wiadomość!
              </Button>
            </form>
          </div>
        ) : (
          <Redirect to="/events" />
        )}
      </div>
    );
  }
}

export default Contact;
