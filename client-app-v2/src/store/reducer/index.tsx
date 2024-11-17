import { ActionTypes } from '../../Common/constants';


export interface UserInfo {
  name: string;
  role: string;
  token: string | null;
}

export interface Article {
  id: string;
  title: string;
  content: string;
  isLiked: boolean;
  // Add other article properties as needed
}

export interface State {
  userInfo: UserInfo;
  authorised: boolean;
  articles: Article[];
  editForm: Article | null;
}

export type Action =
  | { type: ActionTypes.USER_LOGIN; userInfo: UserInfo }
  | { type: ActionTypes.LOAD_ARTICLES; payload: Article[] }
  | { type: ActionTypes.DELETE_ARTICLE; payload: string }
  | { type: ActionTypes.TOGGLE_LIKE; payload: { id: string } }
  | { type: ActionTypes.SETUP_DETAILS_VIEW; payload: Article }
  | { type: ActionTypes.PUBLISH_ARTICLE; payload: Article }
  | { type: ActionTypes.LOGOUT }
  | { type: ActionTypes.CLEAR_ARTICLE }
  | { type: ActionTypes.UPDATE_ARTICLE; payload: Article };

export const initialState: State = {
  userInfo: {
    name: "",
    role: "",
    token: null,
  },
  authorised: false,
  articles: [],
  editForm: null,
};

const saveUserInfo = (state: State, action: { userInfo: UserInfo }): State => ({
  ...state,
  userInfo: action.userInfo,
  authorised: true,
});

const logOut = (): State => {
  sessionStorage.clear();
  window.location.href = "/";
  return initialState;
};

const saveArticles = (state: State, action: { payload: Article[] }): State => ({
  ...state,
  articles: action.payload,
});

const deleteArticle = (state: State, action: { payload: string }): State => ({
  ...state,
  articles: state.articles.filter((x) => x.id !== action.payload),
});

const publishArticles = (state: State, action: { payload: Article }): State => ({
  ...state,
  editForm: null,
  articles: [...state.articles, action.payload],
});

const toggleArticleLike = (state: State, action: { payload: { id: string } }): State => ({
  ...state,
  articles: state.articles.map((t) =>
    t.id === action.payload.id ? { ...t, isLiked: !t.isLiked } : t
  ),
});

const setupDetailsView = (state: State, action: { payload: Article }): State => ({
  ...state,
  editForm: action.payload,
});

const clearArticle = (state: State): State => ({
  ...state,
  editForm: null,
});

const updateArticle = (state: State, action: { payload: Article }): State => {
  const index = state.articles.findIndex((x) => x.id === action.payload.id);
  const updatedArticles = [...state.articles];
  updatedArticles[index] = action.payload;

  return {
    ...state,
    editForm: null,
    articles: updatedArticles,
  };
};

export const reducer = (state: State, action: Action): State => {
  switch (action.type) {
    case ActionTypes.USER_LOGIN:
      return saveUserInfo(state, action);
    case ActionTypes.LOAD_ARTICLES:
      return saveArticles(state, action);
    case ActionTypes.DELETE_ARTICLE:
      return deleteArticle(state, action);
    case ActionTypes.TOGGLE_LIKE:
      return toggleArticleLike(state, action);
    case ActionTypes.SETUP_DETAILS_VIEW:
      return setupDetailsView(state, action);
    case ActionTypes.PUBLISH_ARTICLE:
      return publishArticles(state, action);
    case ActionTypes.LOGOUT:
      return logOut();
    case ActionTypes.CLEAR_ARTICLE:
      return clearArticle(state);
    case ActionTypes.UPDATE_ARTICLE:
      return updateArticle(state, action);
    default:
      return state;
  }
};