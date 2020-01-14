import React from 'react';
import axios from 'axios';
import { getCookie, isUserSignedIn } from 'components/Tools/Tools';
import { EventsItem } from 'components/Events/EventsItem';

class MyEvents extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      events: [],
      signedIn: isUserSignedIn(),
    };
  }

  componentDidMount() {
    axios
      .get(
        process.env.REACT_APP_BACK +
          '/api/GameSessions/GetMyGamesSessions',
        {
          headers: { Authorization: 'Bearer ' + getCookie('token') },
        },
      )
      .then(res => {
        const data = res.data;
        this.setState({ events: data });
      });
  }

  render() {
    return (
      <div className="eventsPage-container">
        <div className="row m-0">
          {this.state.events.map(item => (
            <div className="col-12" key={item.id}>
              <EventsItem item={item} className></EventsItem>
            </div>
          ))}
        </div>
      </div>
    );
  }
}

export default MyEvents;
