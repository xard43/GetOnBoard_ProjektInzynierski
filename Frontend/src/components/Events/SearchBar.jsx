import React from 'react';
import moment from 'moment';
import Datetime from 'react-datetime';
import Button from 'react-bootstrap/Button';

export class SearchBar extends React.Component {
  onSubmitCallback;

  constructor(props) {
    super(props);
    this.state = {
      queryCity: '',
      queryStartDate: '',
      queryEndDate: '',
    };
  }

  search = searchForm => {
    searchForm.preventDefault();
    let searchQuery = {
      searchGameSessionName: this.state.queryCity,
      searchGameSessionDateFrom: this.state.queryStartDate,
      searchGameSessionDateTo: this.state.queryEndDate,
    };
    this.props.onSubmitCallback(searchQuery);
  };

  handleStartDatepickerChange = date => {
    this.setState({
      queryStartDate: moment(date).toISOString(),
    });
  };

  handleEndDatepickerChange = date => {
    this.setState({
      queryEndDate: moment(date).toISOString(),
    });
  };

  handleQueryCityChange = event => {
    this.setState({
      queryCity: event.target.value,
    });
  };

  render() {
    return (
      <div className="searchBar-container rounded bg-light m-0 mb-4">
        <form onSubmit={searchForm => this.search(searchForm)}>
          <div className="form-row">
            <div className="col-sm-10 col-8 pl-4 pt-4 pb-4">
              <input
                type="text"
                placeholder="Nazwa wydarzenia"
                id="city"
                value={this.props.filter}
                onChange={this.props.onChangeFilter}
                className="form-control"
              />
            </div>
            <div className="col-sm-2 col-4 py-4 pr-4 pl-3 text-left">
              <Button
                type="submit"
                className="btn btn-success form-control"
              >
                Znajdź
              </Button>
            </div>
          </div>
          <div className="form-row justify-content-start">
            <div className="col-6 pl-4 pb-4 pr-4">
              <Datetime
                inputProps={{ placeholder: 'Data rozpoczęcia' }}
                dateFormat="DD/MM/YYYY"
                timeFormat="HH:mm"
                onChange={this.handleStartDatepickerChange}
              />
            </div>
            <div className="col-6 pr-4">
              <Datetime
                inputProps={{ placeholder: 'Data zakończenia' }}
                dateFormat="DD/MM/YYYY"
                timeFormat="HH:mm"
                onChange={this.handleEndDatepickerChange}
              />
            </div>
          </div>
        </form>
      </div>
    );
  }
}
