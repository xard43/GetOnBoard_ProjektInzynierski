import React from 'react';
import axios from 'axios';
import { getCookie, isUserSignedIn } from 'components/Tools/Tools';
import { EventsItem } from './EventsItem';
import { SearchBar } from './SearchBar';

class Events extends React.Component {
  constructor(props) {
    super(props);
    this.searchEvents = this.searchEvents.bind(this);
    this.state = {
      events: [],
      filter: '',
      eventsToFilter: [],
      signedIn: isUserSignedIn(),
    };
  }

  searchEvents(searchQuery) {
    axios
      .post(
        process.env.REACT_APP_BACK +
          '/api/GameSessions/SearchGameSessions',
        searchQuery,
        {
          headers: {
            Authorization: 'Bearer ' + getCookie('token'),
          },
        },
      )
      .then(res => {
        const filteredOut = res.data;
        this.setState({ events: filteredOut });
      });
  }

  componentDidMount() {
    axios
      .get(process.env.REACT_APP_BACK + '/api/GameSessions', {
        headers: { Authorization: 'Bearer ' + getCookie('token') },
      })
      .then(res => {
        const data = res.data;
        this.setState({ events: data });
      });
  }

  handleFilterChange = event => {
    this.setState({
      filter: event.target.value,
    });
  };

  render() {
    const { filter, events } = this.state;
    const lowercasedFilter = filter.toLowerCase();
    const filteredEvents = events.filter(item => {
      return Object.keys(item).some(key =>
        item[key]
          .toString()
          .toLowerCase()
          .includes(lowercasedFilter),
      );
    });
    return (
      <div className="eventsPage-container">
        <SearchBar
          filter={this.state.filter}
          onChangeFilter={this.handleFilterChange}
          onSubmitCallback={this.searchEvents.bind(this)}
        />
        <div className="row m-0">
          {filteredEvents.map(item => (
            <div className="col-12" key={item.id}>
              <EventsItem item={item} className></EventsItem>
            </div>
          ))}
        </div>
      </div>
    );
  }
}

export default Events;
