import { actionTypes } from '../../Common/constants'

const saveUserInfo = (state, action) => {
    return {
        ...state,
        userInfo: action.userInfo,
        authorised: true
    }
};

const savePublisherArticles = (state, action) => {
    return {
        ...state,
        publisherArticles: action.payload
    }
};

const toggleArticleLike = (state, action) => {
    return {
        ...state,
        publisherArticles: state.publisherArticles.map((t, idx) =>
            idx === (action.payload.Id - 1) ? { ...t, Like: !t.Like } : t
        )
    };
}

const editArticle = (state, action) => {
    return {
        ...state,
        editForm: action.payload
    };
}

export const reducer = (state, action) => {
    switch (action.type) {
        case actionTypes.USER_LOGIN: return saveUserInfo(state, action);
        case actionTypes.LOAD_PUBLISHER_ARTICLES: return savePublisherArticles(state, action);
        case actionTypes.TOGGLE_LIKE: return toggleArticleLike(state, action);
        case actionTypes.EDIT_ARTICLE: return editArticle(state, action);
        default: return state;
    }



}