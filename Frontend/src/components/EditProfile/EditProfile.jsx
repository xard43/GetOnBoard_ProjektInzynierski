import React from 'react';
import axios from 'axios';
import Image from 'react-bootstrap/Image';
import Button from 'react-bootstrap/Button';
import Form from 'react-bootstrap/Form';
import { getCookie } from 'components/Tools/Tools';
import 'react-confirm-alert/src/react-confirm-alert.css';

class EditProfile extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      profile: [],
      firstName: '',
      lastName: '',
      city: '',
      description: '',
      uploadedAvatar: null,
      uploadedAvatarName: '',
      uploaded: false,
      newAvatarPreview: null,
      newAvatarBase64: '',
    };
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
        this.setState({
          profile: data,
          firstName: data.firstName,
          lastName: data.lastName,
          city: data.city,
          description: data.description,
          newAvatarPreview: data.avatar,
        });
      });
  }

  handleFirstNameChange = e => {
    this.setState({
      firstName: e.target.value,
    });
  };

  handleLastNameChange = e => {
    this.setState({
      lastName: e.target.value,
    });
  };

  handleCityChange = e => {
    this.setState({
      city: e.target.value,
    });
  };

  handleDescriptionChange = e => {
    console.log(e.target);
    this.setState({
      description: e.target.value,
    });
  };

  encodeImageFileAsURL(image) {
    var file = image;
    var reader = new FileReader();
    reader.onloadend = () => {
      var res = reader.result;
      res = res.split(',').pop();
      this.setState({
        newAvatarBase64: res,
      });
    };
    reader.readAsDataURL(file);
  }

  handleAvatarChange = e => {
    if (e.target.files[0]) {
      this.encodeImageFileAsURL(e.target.files[0]);
      this.setState({
        uploadedAvatar: e.target.files[0],
        uploadedAvatarName: e.target.files[0].name,
        uploaded: true,
        newAvatarPreview: URL.createObjectURL(e.target.files[0]),
      });
    }
  };

  handleSubmit = () => {
    var dataToPost = {
      userID: this.state.profile.userID,
      firstName: this.state.firstName,
      lastName: this.state.lastName,
      city: this.state.city,
      description: this.state.description,
      avatar: this.state.newAvatarBase64,
    };

    axios
      .put(process.env.REACT_APP_BACK + '/api/Profile', dataToPost, {
        headers: {
          Authorization: 'Bearer ' + getCookie('token'),
        },
      })
      .then(res => {
        window.location = `/profile/${this.state.profile.userID}`;
      });
  };

  render() {
    return (
      <div id="EditProfileForm" className="bg-white p-5">
        <h1 id="ContactTitle">Edytuj profil</h1>
        <Form.Group className="EditProfile row" controlId="firstName">
          <Form.Label>Imię</Form.Label>
          <Form.Control
            type="text"
            defaultValue={this.state.profile.firstName}
            onChange={this.handleFirstNameChange}
          />
        </Form.Group>

        <Form.Group className="EditProfile row" controlId="lastName">
          <Form.Label>Nazwisko</Form.Label>
          <Form.Control
            type="text"
            defaultValue={this.state.profile.lastName}
            onChange={this.handleLastNameChange}
          />
        </Form.Group>

        <Form.Group className="EditProfile row" controlId="city">
          <Form.Label>Miasto</Form.Label>
          <Form.Control
            type="text"
            defaultValue={this.state.profile.city}
            onChange={this.handleCityChange}
          />
        </Form.Group>

        <Form.Group
          className="EditProfile row"
          controlId="description"
        >
          <Form.Label>Opis</Form.Label>
          <textarea
            value={this.state.description}
            className="form-control"
            rows="5"
            onChange={this.handleDescriptionChange}
            maxLength="500"
          >
            {this.state.description}
          </textarea>
        </Form.Group>

        <Form.Group className="EditProfile row" controlId="avatar">
          {/*<Form.Label>Avatar</Form.Label>*/}

          <div className="input-group mb-3 col-sm-4 col-12">
            <div className="custom-file">
              <label
                className="custom-file-label"
                htmlFor="inputGroupFile01"
              >
                {this.state.uploadedAvatarName
                  ? this.state.uploadedAvatarName
                  : 'Wybierz plik'}
                <input
                  type="file"
                  name="newAvatar"
                  onChange={this.handleAvatarChange}
                  className="custom-file-input"
                  id="inputGroupFile01"
                />
              </label>
            </div>
          </div>
          <Image
            fluid
            style={{ width: '10em', height: '10em' }}
            src={this.state.newAvatarPreview}
            alt="Avatar"
            width="150em"
            height="150em"
            className="col-sm-2 col-8 border"
          />
        </Form.Group>
        <Button
          className="EditProfile row "
          variant="primary"
          onClick={this.handleSubmit}
        >
          Zapisz zmiany
        </Button>
      </div>
    );
  }
}

export default EditProfile;
