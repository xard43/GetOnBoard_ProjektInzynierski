import React from 'react';
// import ReactDOM from 'react-dom';
import 'index.scss';

import GNavbar from 'components/GNavbar/GNavbar';
import AppRouter from 'AppRouter';
import ReactNotification from 'react-notifications-component';
import 'react-notifications-component/dist/theme.css';

require('dotenv').config();

class App extends React.Component {
  render() {
    return (
      <>
        <ReactNotification />
        <div id="app">
          <GNavbar></GNavbar>
          <AppRouter></AppRouter>
        </div>
        {/* <div className="navbar  navbar-dark bg-dark">
          <p className="text-light m-auto">Copyright GetOnBoard </p>
          <ul className="navbar-nav">
            <a className="nav-link " href="/contact">
              Kontakt
            </a>
          </ul>
        </div> */}

        <div class="navbar footer navbar-dark ">
          <p className="text-light m-auto">Copyright GetOnBoard </p>
          <ul className="navbar-nav">
            <a className="nav-link " href="/contact">
              Kontakt
            </a>
          </ul>
        </div>
      </>
    );
  }
}

export default App;
