//function used to read cookies - e.g. use getCookie('token') to get token
export function getCookie(cookiename) {
  var cookiestring = RegExp('' + cookiename + '[^;]+').exec(
    document.cookie,
  );
  return decodeURIComponent(
    !!cookiestring
      ? cookiestring.toString().replace(/^[^=]+./, '')
      : '',
  );
}

//function to check if user is currently logged in or not
export function isUserSignedIn() {
  if (getCookie('token') === '') {
    return false;
  } else {
    return true;
  }
}

//function to log user out by deleting token from cookies
export function logout() {
  getCookie('token');
  document.cookie =
    'token= ; path=/; expires = Thu, 01 Jan 1970 00:00:00 GMT';
  window.location = `login`;
}

//function used to extract username from jwt token
export function getUsername() {
  let token = getCookie('token');
  let payload = JSON.parse(atob(token.split('.')[1]));
  let userName = payload.sub;
  return userName;
}

export function getUserId() {
  let token = getCookie('token');
  let payload = JSON.parse(atob(token.split('.')[1]));
  let userName =
    payload[
      'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'
    ];
  return userName;
}
