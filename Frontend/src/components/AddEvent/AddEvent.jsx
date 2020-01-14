import React from 'react';
import Button from 'react-bootstrap/Button';
import Image from 'react-bootstrap/Image';
import Form from 'react-bootstrap/Form';
import axios from 'axios';
import moment from 'moment';
import 'components/css/react-datetime.scss';
import './AddEvent.scss';
import './AddEventMobile.css';

// import Select from 'react-select'; //for Select validation (required field)
import BaseSelect from 'react-select';
import { GobSelect } from './GobSelect.jsx';
import FixRequiredSelect from './AddEvent_FixRequiredSelect.jsx'; //for Select validation (required field)
import pl from 'date-fns/locale/pl';
import { registerLocale } from 'react-datepicker';
import DatePicker from 'react-datepicker';
import './react-datepicker.scss';
import { getCookie } from 'components/Tools/Tools';
import 'moment/locale/pl';

registerLocale('pl', pl);
moment.locale('pl');
moment().format('LLL');

const slotsList = [
  { value: 2, label: '2' },
  { value: 3, label: '3' },
  { value: 4, label: '4' },
  { value: 5, label: '5' },
  { value: 6, label: '6' },
];

const options = [
  { value: 1, label: '1 - One' },
  { value: 2, label: '2 - Two' },
  { value: 3, label: '3 - Three' },
];

const Select = props => (
  <FixRequiredSelect
    {...props}
    SelectComponent={GobSelect}
    options={props.options || options}
  />
);

class AddEvent extends React.Component {
  constructor(props) {
    super(props);
    this.handleSubmit = this.handleSubmit.bind(this);
    this.state = {
      minTime: this.calculateMinTime(new Date()),
      startTime: moment(moment.now()).toISOString(),
      game: 0,
      slots: 2,
      letUserAddGame: false,
      customGameImage: '',
      customGameImageName: '',
      customGameName: '',
      customGameImageBase64: '',
      uploadedImage: null,
      gamesList: [],
      closeDate: moment(moment.now())
        .add({ hours: 1 })
        .toDate(),
    };
  }

  calculateMinTime = date => {
    let isToday = moment(date).isSame(moment(), 'day');
    if (isToday) {
      let nowAddOneHour = moment(new Date())
        .add({ hours: 1 })
        .toDate();
      return nowAddOneHour;
    }
    return moment()
      .startOf('day')
      .toDate(); // set to 12:00 am today
  };

  componentDidMount() {
    axios
      .get(
        process.env.REACT_APP_BACK +
          '/api/GameSessions/GetDictionaryBoardGames/',
        {
          headers: {
            Authorization: 'Bearer ' + getCookie('token'),
          },
        },
      )
      .then(res => {
        const data = res.data;
        this.setState({ gamesList: data });
      });
  }
  handleDateChange = date => {
    this.setState({
      closeDate: date,
      minTime: this.calculateMinTime(date),
      startTime: moment(date).toISOString(),
    });
  };
  handleDatepickerChange = date => {
    this.setState({
      startTime: moment(date).toISOString(),
    });
  };

  handleGameChange = dictPosition => {
    console.log(dictPosition.imageBoardGame);
    this.setState({
      game: dictPosition.value,
      customGameImage: dictPosition.imageBoardGame,
    });
  };

  handleSlotsChange = dictPosition => {
    this.setState({
      slots: dictPosition.value,
    });
  };

  handleSubmit(event) {
    var dataToPost = {
      city: event.target.city.value,
      address: event.target.address.value,
      name: event.target.name.value,
      description: event.target.description.value,
      timeStart: this.state.startTime,
      slots: this.state.slots,
      boardGameID: this.state.game,
      isCustomGame: this.state.letUserAddGame,
      customGameName: this.state.customGameName,
      customGameImage: this.state.customGameImageBase64,
    };

    axios
      .post(
        process.env.REACT_APP_BACK + '/api/GameSessions',
        dataToPost,
        {
          headers: {
            Authorization: 'Bearer ' + getCookie('token'),
          },
        },
      )
      .then(res => {
        const data = res.data;
        window.location = `/event/` + data.id;
      });
  }

  addGame = () => {
    this.setState(prevState => ({
      letUserAddGame: !prevState.letUserAddGame,
    }));
  };

  handleCustomImageChange = e => {
    if (e.target.files[0]) {
      this.encodeImageFileAsURL(e.target.files[0]);
      this.setState({
        uploadedImage: e.target.files[0],
        customGameImage: URL.createObjectURL(e.target.files[0]),
        customGameImageName: e.target.files[0].name,
      });
    }
  };

  handleCustomGameNameChange = e => {
    this.setState({
      customGameName: e.target.value,
    });
  };

  handleDateChangeRaw = e => {
    e.preventDefault();
  };

  encodeImageFileAsURL(image) {
    var file = image;
    var reader = new FileReader();
    reader.onloadend = () => {
      var res = reader.result;
      res = res.split(',').pop();
      this.setState({
        customGameImageBase64: res,
      });
    };
    reader.readAsDataURL(file);
  }

  render() {
    return (
      <div className="w-100">
        <div id="AddEventForm">
          <h1 id="AddEventTitle">Stwórz wydarzenie</h1>
          <form
            onSubmit={event => {
              event.preventDefault();
              this.handleSubmit(event);
            }}
            className="mx-5"
          >
            <div className="form-row">
              <Form.Group
                className="AddEventInput col"
                controlId="name"
              >
                <Form.Label>Nazwa </Form.Label>
                <Form.Control
                  type="text"
                  placeholder="Krótka nazwa spotkania"
                  required={true}
                  maxLength="40"
                />
              </Form.Group>

              <Form.Group
                className="AddEventInput col"
                controlId="city"
              >
                <Form.Label>Miasto</Form.Label>
                <Form.Control
                  type="text"
                  placeholder="np. Poznań, Warszawa"
                  required={true}
                />
              </Form.Group>

              <Form.Group
                className="AddEventInput col"
                controlId="address"
              >
                <Form.Label>Adres</Form.Label>
                <Form.Control
                  type="text"
                  placeholder="np. Wodna 6/13"
                  required={true}
                />
              </Form.Group>
            </div>
            <div className="form-row">
              <Form.Group
                className="AddEventInput col-sm-7"
                controlId="startTime"
              >
                <Form.Label>Data i czas</Form.Label>
                <br />

                <DatePicker
                  className="form-control customDatePickerWidth"
                  onChangeRaw={this.handleDateChangeRaw}
                  onChange={this.handleDateChange}
                  selected={this.state.closeDate}
                  minDate={moment.now()}
                  minTime={this.state.minTime}
                  maxTime={moment()
                    .endOf('day')
                    .toDate()} // set to 23:59 pm today
                  // timeIntervals={15}
                  showTimeSelect
                  locale="pl"
                  dateFormat="dd MMMM yyyy HH:mm"
                  timeFormat="HH:mm"
                  timeCaption="godzina"
                  placeholderText="Wybierz datę spotkania"
                />
              </Form.Group>

              <Form.Group
                className="AddEventInput col-sm-4"
                controlId="slots"
              >
                <Form.Label>Miejsca</Form.Label>
                <GobSelect
                  options={slotsList}
                  onChange={this.handleSlotsChange}
                  placeholder="Ilu graczy?"
                  required={true}
                />
              </Form.Group>
            </div>
            <div className="form-row">
              <div className="col-sm-7">
                <Form.Group
                  className="AddEventInput "
                  controlId="description"
                >
                  <Form.Label>Opis</Form.Label>
                  <Form.Control
                    type="textarea"
                    placeholder="Napisz coś o tym spotkaniu"
                    required={true}
                    maxLength="200"
                    className="form-control"
                    rows="4"
                  />
                </Form.Group>
                <Form.Group className="AddEventInput">
                  <Form.Label>Gra</Form.Label>
                </Form.Group>
                {this.state.letUserAddGame ? (
                  <div>
                    <Form.Group
                      className="AddEventInput"
                      controlId="customGameName"
                    >
                      <Form.Control
                        type="text"
                        placeholder="Nazwa Twojej gry"
                        onChange={this.handleCustomGameNameChange}
                        required={true}
                      />
                    </Form.Group>

                    <Form.Group
                      className="AddEventInput"
                      controlId="customGameImage"
                    >
                      <Form.Label>Obrazek gry</Form.Label>
                      <div className="custom-file">
                        <input
                          type="file"
                          name="newCustomGameImage"
                          className="custom-file-input"
                          id="inputGroupFile01"
                          onChange={this.handleCustomImageChange}
                        />
                        <label
                          className="custom-file-label"
                          htmlFor="inputGroupFile01"
                        >
                          {this.state.customGameImageName
                            ? this.state.customGameImageName
                            : 'Wybierz plik'}
                        </label>
                      </div>
                    </Form.Group>
                    <Form.Group className="AddEventInput">
                      <Button
                        className="outline-info  form-control mb-3"
                        variant="outline-info"
                        onClick={this.addGame}
                      >
                        Wybierz grę z listy
                      </Button>
                    </Form.Group>
                  </div>
                ) : (
                  <Form.Group
                    className="AddEventInput"
                    controlId="game"
                  >
                    <GobSelect
                      options={this.state.gamesList}
                      onChange={this.handleGameChange}
                      placeholder="Wybierz grę z listy"
                      isSearchable
                      required={true}
                    />
                    <Button
                      variant="outline-info  form-control mt-4 "
                      onClick={this.addGame}
                    >
                      Dodaj własną grę!
                    </Button>
                  </Form.Group>
                )}
              </div>
              <Form.Group className="AddEventInput col-sm-4 ">
                <div className="float-none">
                  <Form.Label>Zdjęcie gry</Form.Label>
                  <div>
                    {this.state.customGameImage ? (
                      <Image
                        fluid
                        src={this.state.customGameImage}
                        alt="MyGame"
                        width="200em"
                        height="200em"
                        className="border"
                      />
                    ) : (
                      <Image
                        // fluid
                        src={require('./../../img/image-placeholder.png')}
                        alt="MyGame"
                        width="200em"
                        height="200em"
                        className="border"
                      />
                    )}
                  </div>
                </div>
              </Form.Group>
            </div>
            <div className="form-row">
              <Button
                className="AddEventInput"
                variant="primary"
                type="submit"
              >
                Utwórz
              </Button>
            </div>
          </form>
        </div>
      </div>
    );
  }
}

export default AddEvent;
