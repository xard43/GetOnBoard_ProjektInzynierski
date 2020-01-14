import React from 'react';
import moment from 'moment';
import LinesEllipsis from 'react-lines-ellipsis';

import './EventsItem.scss';

export class EventsItem extends React.Component {
  render() {
    return (
      <div
        className="eventsItem-container rounded text-dark row mb-4"
        style={{}}
        onClick={() => {
          window.location = `/event/${this.props.item.id}`;
        }}
      >
        <div className="col-lg-2 rounded-left m-auto col-sm-4 col-12">
          <img
            className="rounded pt-3 pl-3"
            src={this.props.item.gameAvatar}
            alt="Generic Placeholder"
            width="150px"
            height="200px"
          />
        </div>
        <div className="col-lg-10 col-sm-8 col-12">
          <div className="row card-title h2 text-truncate m-0 pt-2 pb-3 px-2 mw-100">
            {this.props.item.name}
          </div>
          <div className="row m-0 p-0 card-text">
            <div className="col-lg-4 col-sm-12 col-12 ">
              <div className="row">
                <div className="col-lg-3 col-sm-3 col-3">
                  <p className="">Miasto: </p>
                  <p className="">Adres: </p>
                  <p className="">Kiedy: </p>
                  <p className="">Gra: </p>
                </div>
                <div className="col-lg-9 col-sm-9 col-9">
                  <p className=" text-truncate">
                    {this.props.item.city}
                  </p>
                  <p className=" text-truncate">
                    {this.props.item.address}
                  </p>
                  <p className=" text-truncate">
                    {moment(this.props.item.timeStart)
                      .utcOffset(2)
                      .format('DD-MM-YYYY, HH:mm')}
                  </p>
                  <p className="row text-truncate">
                    {this.props.item.gameName}
                  </p>
                </div>
              </div>
            </div>
            <div className="col-lg-1 col-sm-3 col-3">
              <p className="row">Opis: </p>
            </div>
            <div className="col-lg-3 col-sm-9 col-9 text-truncate ">
              <LinesEllipsis
                style={{ whiteSpace: 'pre-wrap' }}
                text={this.props.item.description}
                maxLine="4"
                basedOn="words"
                component="p"
              />
            </div>
            <div
              className="col-lg-4 col-sm-12 col-12 players-placeholder rounded-right mb-3 "
              align="left"
            >
              <p className="row">Gracze: </p>
              {this.props.item.players.map(player => {
                return (
                  <img
                    key={player.userName}
                    width="50em"
                    height="50em"
                    src={player.avatar}
                    alt={player.userName}
                    about={player.username}
                    className="rounded-circle border m-1"
                  />
                );
              })}
              <div className="row">{this.props.item.UserAdminID}</div>
            </div>
          </div>
        </div>
      </div>
    );
  }
}
