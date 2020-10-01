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

const publishArticles = (state, action) => {
    const newArticle = state.publisherArticles
    newArticle.push({
        ...action.payload
    })
    return {
        ...state,
        publisherArticles: newArticle
    }
};


const toggleArticleLike = (state, action) => {
    return {
        ...state,
        publisherArticles: state.publisherArticles.map((t, idx) =>
           // idx === (action.payload.id - 1) ? { ...t, Like: !t.Like } : t
           t.id === (action.payload.id) ? { ...t, Like: !t.Like } : t
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
        case actionTypes.PUBLISH_ARTICLE: return publishArticles(state, action);
        default: return state;
    }



}