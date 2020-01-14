import React from 'react';
import { Route, BrowserRouter as Router } from 'react-router-dom';
import Home from 'components/Home/Home';
import Register from 'components/Register/Register';
import Events from 'components/Events/Events';
import MyEvents from 'components/MyEvents/MyEvents';
import Event from 'components/Event/Event';
import AddEvent from 'components/AddEvent/AddEvent';
import Profile from 'components/Profile/Profile';
import Contact from 'components/Contact/Contact';
import EditProfile from 'components/EditProfile/EditProfile';
import PrivateRoute from 'components/PrivateRoute/PrivateRoute';
import { FrontPage } from './components/FrontPage/FrontPage';
import { Switch } from 'react-router-dom';

class AppRouter extends React.Component {
  render() {
    return (
      <Router>
        <Switch>
          {/*container moved to component Home*/}
          <Route exact path="/" component={Home} />
          <Route path="/login" component={FrontPage} />
          <Route path="/register" component={Register} />
          <Route path="/contact" component={Contact} />
          {/*<Route path="/login" component={Login} />*/}
          <PrivateRoute path="/events" component={Events} />
          <PrivateRoute path="/myevents" component={MyEvents} />
          <PrivateRoute path="/addevent" component={AddEvent} />
          <PrivateRoute path="/event/:id" component={Event} />
          <PrivateRoute path="/profile/:id" component={Profile} />
          <PrivateRoute
            path="/editprofile/:id"
            component={EditProfile}
          />
          <Route component={Home} />
        </Switch>
      </Router>
    );
  }
}

export default AppRouter;
