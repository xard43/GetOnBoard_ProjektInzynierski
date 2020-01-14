import React from 'react';
import Button from 'react-bootstrap/Button';
import Form from 'react-bootstrap/Form';
import axios from 'axios';
import './Login.scss';
import { store } from 'react-notifications-component';

class Login extends React.Component {
  constructor(props) {
    super(props);
    this.handleSubmit = this.handleSubmit.bind(this);
    this.state = {
      username: '',
      usernameError: '',
      password: '',
      passwordError: '',
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
        username.length > 0 ? null : 'Wprowadź nazwę użytkownika',
    });
  };

  handlePasswordChange = event => {
    this.setState({ password: event.target.value }, () => {
      this.validatePassword();
    });
  };
  validatePassword = () => {
    const { password } = this.state;
    this.setState({
      passwordError: password.length > 0 ? null : 'Wprowadź hasło',
    });
  };

  handleSubmit(event) {
    event.preventDefault();
    var dataToPost = {
      username: event.target.username.value,
      password: event.target.password.value,
    };

    axios
      .post(process.env.REACT_APP_BACK + '/login', dataToPost)
      .then(res => {
        const response = res.data;
        var expires = new Date(response.expiration).toUTCString();
        document.cookie =
          'token=' + response.token + '; path=/;  expires=' + expires;
        window.location = `/events`;
      })
      .catch(reason => {
        store.addNotification({
          title: 'Logowanie nieudane',
          message: 'Wprowadzono błędny login lub hasło',
          type: 'danger',
          insert: 'top',
          container: 'bottom-right',
          animationIn: ['animated', 'fadeIn'],
          animationOut: ['animated', 'fadeOut'],
          dismiss: {
            duration: 2000,
          },
        });
      });
  }

  render() {
    return (
      <div id="LoginForm" className="p-2">
        <form onSubmit={this.handleSubmit.bind(this)}>
          <Form.Group controlId="username" className="LoginInput">
            <div className="p-0">
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

              {/*<Form.Control.Feedback type="invalid">*/}
              {/*  {this.state.usernameError}*/}
              {/*</Form.Control.Feedback>*/}
            </div>
          </Form.Group>

          <Form.Group controlId="password" className="LoginInput">
            <Form.Control
              type="password"
              className={`form-control ${
                this.state.passwordError ? 'is-invalid' : ''
              }`}
              value={this.state.password}
              onChange={this.handlePasswordChange}
              onBlur={this.validatePassword}
              placeholder="Hasło"
            />
            {/*<Form.Control.Feedback type="invalid">*/}
            {/*  {this.state.passwordError}*/}
            {/*</Form.Control.Feedback>*/}
          </Form.Group>

          <Button
            className="LoginInput"
            variant="primary"
            disabled={
              this.state.username.length < 1 ||
              this.state.password.length < 1
            }
            type="submit"
          >
            Zaloguj
          </Button>
        </form>
      </div>
    );
  }
}

export default Login;
