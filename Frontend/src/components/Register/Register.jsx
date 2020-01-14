import React from 'react';
import Button from 'react-bootstrap/Button';
import Form from 'react-bootstrap/Form';
import axios from 'axios';
import './Register.scss';
import { store } from 'react-notifications-component';

class Register extends React.Component {
  constructor(props) {
    super(props);
    this.handleSubmit = this.handleSubmit.bind(this);
    this.state = {
      username: '',
      usernameError: '',
      newEmail: '',
      newEmailError: '',
      newPassword: '',
      newPasswordError: '',
      newPasswordConfirmation: '',
      newPasswordConfirmationError: '',
    };
  }

  handleUsernameChange = event => {
    this.setState({ username: event.target.value }, () => {
      this.validateUsername();
    });
  };
  validateUsername = () => {
    const { username } = this.state;
    this.setState({
      usernameError:
        username.length > 3 && username.length < 21
          ? null
          : 'Nazwa musi zawierać od 4 do 20 znaków',
    });
  };

  handleNewEmailChange = event => {
    this.setState({ newEmail: event.target.value }, () => {
      this.validateNewEmail();
    });
  };
  validateNewEmail = () => {
    const { newEmail } = this.state;
    this.setState({
      newEmailError:
        newEmail.length > 3 && newEmail.includes('@')
          ? null
          : "Email musi mieć conajmniej 4 znaki i zawierać '@'",
    });
  };

  handleNewPasswordChange = event => {
    this.setState({ newPassword: event.target.value }, () => {
      this.validateNewPassword();
    });
  };
  validateNewPassword = () => {
    const { newPassword } = this.state;
    this.setState({
      newPasswordError:
        newPassword.length > 5
          ? null
          : 'Hasło musi mieć conajmniej 6 znaków',
    });
  };

  handleNewPasswordConfirmationChange = event => {
    this.setState(
      { newPasswordConfirmation: event.target.value },
      () => {
        this.validateNewPasswordConfirmation();
      },
    );
  };
  validateNewPasswordConfirmation = () => {
    const { newPasswordConfirmation } = this.state;
    this.setState({
      newPasswordConfirmationError:
        newPasswordConfirmation === this.state.newPassword
          ? null
          : 'Hasła muszą być takie same',
    });
  };

  handleSubmit(event) {
    event.preventDefault();
    var dataToPost = {
      username: event.target.username.value,
      email: event.target.newEmail.value,
      password: event.target.newPassword.value,
    };
    axios
      .post(process.env.REACT_APP_BACK + '/register', dataToPost)
      .then(res => {
        const response = res.data;
        var expires = new Date(response.value.expiration).toUTCString();
        document.cookie =
          'token=' + response.value.token + '; path=/;  expires=' + expires;
        window.location = `/events`;
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
      <div id="RegisterForm">
        <h1 id="RegisterTitle">Zarejestruj się</h1>
        <form onSubmit={this.handleSubmit}>
          <Form.Group className="RegisterInput" controlId="username">
            <Form.Label>Nazwa użytkownika</Form.Label>
            <Form.Control
              type="text"
              className={`form-control ${
                this.state.usernameError ? 'is-invalid' : ''
              }`}
              value={this.state.username}
              onChange={this.handleUsernameChange}
              onBlur={this.validateUsername}
              placeholder="Nazwa użytkownika"
            />
            <Form.Control.Feedback type="invalid">
              {this.state.usernameError}
            </Form.Control.Feedback>
          </Form.Group>

          <Form.Group className="RegisterInput" controlId="newEmail">
            <Form.Label>Adres email</Form.Label>
            <Form.Control
              type="email"
              className={`form-control ${
                this.state.newEmailError ? 'is-invalid' : ''
              }`}
              value={this.state.NewEmail}
              onChange={this.handleNewEmailChange}
              onBlur={this.validateNewEmail}
              placeholder="użytkownik@domena.com"
            />
            <Form.Control.Feedback type="invalid">
              {this.state.newEmailError}
            </Form.Control.Feedback>
          </Form.Group>

          <Form.Group
            className="RegisterInput"
            controlId="newPassword"
          >
            <Form.Label>Hasło</Form.Label>
            <Form.Control
              type="password"
              className={`form-control ${
                this.state.newPasswordError ? 'is-invalid' : ''
              }`}
              value={this.state.NewPassword}
              onChange={this.handleNewPasswordChange}
              onBlur={this.validateNewPassword}
              placeholder="Hasło"
            />
            <Form.Control.Feedback type="invalid">
              {this.state.newPasswordError}
            </Form.Control.Feedback>
          </Form.Group>

          <Form.Group
            className="RegisterInput"
            controlId="newPasswordConfirmation"
          >
            <Form.Label>Potwierdź hasło</Form.Label>
            <Form.Control
              type="password"
              disabled={this.state.newPasswordError}
              className={`form-control ${
                this.state.newPasswordConfirmationError
                  ? 'is-invalid'
                  : ''
              }`}
              value={this.state.NewPasswordConfirmation}
              onChange={this.handleNewPasswordConfirmationChange}
              onBlur={this.validateNewPasswordConfirmation}
              placeholder="Hasło"
            />
            <Form.Control.Feedback type="invalid">
              {this.state.newPasswordConfirmationError}
            </Form.Control.Feedback>
          </Form.Group>
          <Button
            className="RegisterInput"
            variant="primary"
            type="submit"
            disabled={
              this.state.usernameError ||
              this.state.newEmailError ||
              this.state.newPasswordError ||
              this.state.newPasswordConfirmationError ||
              this.state.username === '' ||
              this.state.newEmail === '' ||
              this.state.newPassword === '' ||
              this.state.newPasswordConfirmation === ''
            }
          >
            Stwórz konto
          </Button>
        </form>
      </div>
    );
  }
}

export default Register;
