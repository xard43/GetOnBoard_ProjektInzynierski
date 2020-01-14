import React from 'react';
import { Route, Redirect } from 'react-router-dom';
import { isUserSignedIn } from 'components/Tools/Tools';

export const PrivateRoute = ({ component: Component, ...rest }) => (
  <Route
    {...rest}
    render={props =>
      isUserSignedIn() ? (
        <div className="container">
          <Component {...props} />
        </div>
      ) : (
        <Redirect
          to={{ pathname: '/login', state: { from: props.location } }}
        />
      )
    }
  />
);

export default PrivateRoute;
