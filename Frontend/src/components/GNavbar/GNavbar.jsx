import React from 'react';
import {
  getUsername,
  isUserSignedIn,
  logout,
} from 'components/Tools/Tools';
import {
  Nav,
  Navbar,
  DropdownButton,
  Dropdown,
} from 'react-bootstrap';
import { getUserId } from '../Tools/Tools';
import Login from '../Login/Login';

class GNavbar extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      signedIn: isUserSignedIn(),
    };
  }

  render() {
    if (!this.state.signedIn) {
      return (
        <Navbar
          collapseOnSelect
          expand="lg"
          bg="dark"
          variant="dark"
          fixed="top"
        >
          <Navbar.Brand href="/">GetOnBoard</Navbar.Brand>
          <Navbar.Toggle aria-controls="responsive-navbar-nav" />
          <Navbar.Collapse id="responsive-navbar-nav">
            <Nav className="mr-auto"></Nav>
            <Nav>
              <div className="mr-4">
                <DropdownButton
                  title="login"
                  variant="secondary"
                  alignRight
                >
                  <Login></Login>
                </DropdownButton>
              </div>
              <Nav.Link href="/register">Zarejestruj</Nav.Link>
            </Nav>
          </Navbar.Collapse>
        </Navbar>
      );
    } else
      return (
        <Navbar
          collapseOnSelect
          expand="lg"
          bg="dark"
          variant="dark"
          fixed="top"
        >
          <Navbar.Brand href="/">GetOnBoard</Navbar.Brand>
          <Navbar.Toggle aria-controls="responsive-navbar-nav" />
          <Navbar.Collapse id="responsive-navbar-nav">
            <Nav className="mr-auto">
              <Nav.Link href="/events">Wydarzenia</Nav.Link>
              <Nav.Link href="/addevent">Dodaj wydarzenie</Nav.Link>
            </Nav>
            <Nav>
              <DropdownButton
                title={getUsername()}
                variant="secondary"
                alignRight
              >
                <Dropdown.Item href={'/profile/' + getUserId()}>
                  Mój profil
                </Dropdown.Item>
                <Dropdown.Item href="/myevents">
                  Moje wydarzenia
                </Dropdown.Item>
                <Dropdown.Divider />
                <Dropdown.Item onClick={() => logout()}>
                  Wyloguj
                </Dropdown.Item>
              </DropdownButton>
            </Nav>
          </Navbar.Collapse>
        </Navbar>
      );
  }
}

export default GNavbar;
