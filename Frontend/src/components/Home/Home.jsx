import React from 'react';
import { isUserSignedIn } from 'components/Tools/Tools';
import { Redirect } from 'react-router';

class Home extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      signedIn: isUserSignedIn(),
    };
  }

  render() {
    if (!this.state.signedIn) {
      return <Redirect to="/login" />;
    } else {
      return <Redirect to="/events" />;
    }
  }
}

export default Home;
