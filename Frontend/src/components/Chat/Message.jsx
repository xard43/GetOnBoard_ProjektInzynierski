import React from 'react';
import moment from 'moment';
import './Message.scss';

export class Message extends React.Component {
  render() {
    return (
      <div className="row mb-2">
        <div className="col-xl-2 col-lg-2	col-md-3 col-sm-3 col-4 text-center user-img p-0">
          <img
            id="profile-photo"
            src={this.props.messageObject.avatar}
            className="rounded-circle float-right mr-3"
            alt={this.props.messageObject.author} //has to be added, so I thought author would be ok
          />
        </div>
        <div className=" col-xl-10 col-lg-10 col-md-9 col-sm-9 col-8 message rounded mb-2 ">
          <h4 className="mb-2">{this.props.messageObject.author}</h4>
          <time className="text-white ml-3 float-right">
            {moment(this.props.messageObject.sendTime).calendar()}
          </time>
          <p className="mb-0 text-white">
            {this.props.messageObject.messageContent}
          </p>
        </div>
      </div>
    );
  }
}
