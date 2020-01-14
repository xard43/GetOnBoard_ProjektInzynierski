import Login from '../Login/Login';
import React from 'react';
import { IconContext } from 'react-icons';
import * as FaIcons from 'react-icons/fa';

import './FrontPage.css';
import './FrontPageMobile.css';

export class FrontPage extends React.Component {
  render() {
    return (
      <div className="  front-page-placeholder bg-light">
        <div className=" jumbotron-fluid front-page-top-image">
          <div className="row justify-content-center">
            <div className="col d-inline text-center">
              <div className="d-inline-block">
                <h2 className="front-page-title ">GetOnBoard</h2>
              </div>
              <div className="d-inline-block">
                <IconContext.Provider
                  value={{
                    color: 'white',
                    className: ' p-3',
                    size: '6em',
                  }}
                >
                  <div className="">
                    <FaIcons.FaShip />
                  </div>
                </IconContext.Provider>
              </div>
            </div>
          </div>
          <div className="row justify-content-center">
            <div className="col-lg-3 mx-auto text-center mb-2 text-light">
              <h3>
                Wszystkie wydarzenia planszówkowe w jednym miejscu
              </h3>
            </div>
          </div>
        </div>
        <section className="testimonials text-center bg-light m-0 p-0 pb-4">
          <div className="container">
            <h2 className="mb-5">
              GetOnBoard to miejsce zrzeszające miłośników gier
              planszowych
            </h2>
            <div className="row">
              <div className="col-lg-4">
                <div className="testimonial-item mx-auto mb-5 mb-lg-0">
                  <div className="rounded-circle image-placeholder mx-auto">
                    <IconContext.Provider
                      value={{
                        color: 'white',
                        className: ' p-3',
                        size: '6em',
                      }}
                    >
                      <div className="">
                        <FaIcons.FaMapMarkedAlt />
                      </div>
                    </IconContext.Provider>
                  </div>
                  <h5>Znajdź wydarzenia w twojej okolicy</h5>
                  <p className="font-weight-light mb-0">
                    Wszystkie wydarzenie planszówkowe w jednym miejscu
                  </p>
                </div>
              </div>
              <div className="col-lg-4">
                <div className="testimonial-item mx-auto mb-5 mb-lg-0">
                  <div className="rounded-circle image-placeholder mx-auto">
                    <IconContext.Provider
                      value={{
                        color: 'white',
                        className: ' p-3',
                        size: '6em',
                      }}
                    >
                      <div className="">
                        <FaIcons.FaChessPawn />
                      </div>
                    </IconContext.Provider>
                  </div>
                  <h5>Twórz wydarzenia</h5>
                  <p className="font-weight-light mb-0">
                    Zapraszaj znajomych lub znajdź graczy w twojej
                    okolicy
                  </p>
                </div>
              </div>
              <div className="col-lg-4">
                <div className="testimonial-item mx-auto mb-5 mb-lg-0">
                  <div className="rounded-circle image-placeholder mx-auto">
                    <IconContext.Provider
                      value={{
                        color: 'white',
                        className: ' p-3',
                        size: '6em',
                      }}
                    >
                      <div className="">
                        <FaIcons.FaShare />
                      </div>
                    </IconContext.Provider>
                  </div>
                  <h5>Udostępniaj</h5>
                  <p className="font-weight-light mb-0">
                    Dziel się wydarzeniami ze znajomymi za
                    pośrednictwem portali społecznościowych.
                  </p>
                </div>
              </div>
            </div>
          </div>
        </section>
      </div>
    );
  }
}
