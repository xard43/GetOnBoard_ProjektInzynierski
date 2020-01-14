import React from 'react';
import Image from 'react-bootstrap/Image';
import axios from 'axios';
import { getCookie, isUserSignedIn } from 'components/Tools/Tools';
import './Profile.scss';
import './ProfileMobile.scss';
import './pencil.png';
import 'react-confirm-alert/src/react-confirm-alert.css';

class Profile extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      profile: [],
      signedIn: isUserSignedIn(),
      modal: false,
    };
    this.toggle = this.toggle.bind(this);
  }
  toggle() {
    this.setState(prevState => ({
      modal: !prevState.modal,
    }));
  }

  componentDidMount() {
    axios
      .get(
        process.env.REACT_APP_BACK +
          '/api/Profile/' +
          this.props.match.params.id,
        {
          headers: {
            Authorization: 'Bearer ' + getCookie('token'),
          },
        },
      )
      .then(res => {
        const data = res.data;
        this.setState({ profile: data });
      });
  }

  handleEdit = () => {
    window.location = `/editprofile/${this.state.profile.userID}`;
  };

  render() {
    return (
      <div id="ProfileBody">
        <div id="ProfileHeader">
          {/* Nazwa gracza: */}
          <div id="ProfileHeaderLabel">
            <h1>
              Profil gracza{' '}
              <strong> {this.state.profile.userName} </strong>
            </h1>
          </div>
          {this.state.profile.isItMyProfile ? (
            <div id="ProfileEdit">
              <a href className="ProfileA" onClick={this.handleEdit}>
                <div id="ProfileEditImage"></div>
              </a>
            </div>
          ) : (
            ''
          )}
        </div>
        <div id="ProfileLeftMargin">
          <div id="ProfileAvatar">
            <Image
              fluid
              width="200px"
              src={this.state.profile.avatar}
              alt={this.state.profile.userName}
              rounded
              onClick={this.toggle}
            >
              {this.props.buttonLabel}
            </Image>
            <br></br>
          </div>
          {this.state.profile.userName}
        </div>
        <div id="ProfilePersonal">
          <div id="ProfilePersonalLabel">DANE: </div>

          <div id="ProfilePersonal1">
            <div id="ProfilePersonal1Label">Imię:</div>
            <div id="ProfilePersonal1Data">
              {this.state.profile.firstName}
            </div>
          </div>
          <div id="ProfilePersonal2">
            <div id="ProfilePersonal2Label">Nazwisko:</div>
            <div id="ProfilePersonal2Data">
              {this.state.profile.lastName}
            </div>
          </div>
          <div id="ProfilePersonal3">
            <div id="ProfilePersonal3Label">Miasto:</div>
            <div id="ProfilePersonal3Data">
              {this.state.profile.city}
            </div>
          </div>
          <div id="ProfilePersonal4">
            <div id="ProfilePersonal4Label">Opis:</div>
            <div id="ProfilePersonal4Data">
              {this.state.profile.description}
            </div>
          </div>
        </div>
        <div id="ProfileStats">
          <div id="ProfileStatsLabel">STATYSTYKI:</div>
          <div id="ProfileStats1">
            <div id="ProfileStats1Label">
              Liczba utworzonych wydarzeń:
            </div>
            <div id="ProfileStats1Data">
              {this.state.profile.numberOfGamesSessionCreated}
            </div>
          </div>
          <div id="ProfileStats2">
            <div id="ProfileStats2Label">
              Liczba wydarzeń, w których uczestniczył:
            </div>
            <div id="ProfileStats2Data">
              {this.state.profile.numberOfGamesSessionJoined}
            </div>
          </div>
          <div id="ProfileStats3">
            <div id="ProfileStats3Label">
              Liczba wydarzeń, które opuścił:
            </div>
            <div id="ProfileStats3Data">
              {this.state.profile.numberOfGamesSessionLeft}
            </div>
          </div>
          <div id="ProfileStats4">
            <div id="ProfileStats4Label">
              Liczba wydarzeń, które anulował:
            </div>
            <div id="ProfileStats4Data">
              {this.state.profile.numberOfGamesSessionDeletedasAdmin}
            </div>
          </div>
          <div id="ProfileStats5">
            <div id="ProfileStats5Label">
              Liczba wydarzeń, z których został wyrzucony:
            </div>
            <div id="ProfileStats5Data">
              {
                this.state.profile
                  .numberOfGamesSessionYouWereKickedOut
              }
            </div>
          </div>
        </div>
      </div>
    );
  }
}

export default Profile;
