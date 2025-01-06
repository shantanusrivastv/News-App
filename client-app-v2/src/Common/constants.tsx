export const Configs = {
  URL: window.REACT_APP_CONFIG?.API_URL || "http://localhost:63961/api/",
};

export enum ActionTypes {
  USER_LOGIN = "USER_LOGIN",
  LOAD_ALL_ARTICLES = "LOAD_ALL_ARTICLES",
  LOAD_ARTICLES = "LOAD_PUBLISHER_ARTICLES",
  DELETE_ARTICLE = "DELETE_ARTICLE",
  SETUP_DETAILS_VIEW = "EDIT_ARTICLE",
  UPDATE_ARTICLE = "UPDATE_ARTICLE",
  TOGGLE_LIKE = "TOGGLE_LIKE",
  PUBLISH_ARTICLE = "PUBLISH_ARTICLE",
  LOGOUT = "LOGOUT",
  CLEAR_ARTICLE = "CLEAR_ARTICLE",
}

export enum RoleType {
  PUBLISHER = "Publisher",
  USER = "User",
}