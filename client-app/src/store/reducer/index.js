import { actionTypes } from '../../Common/constants'

const saveUserInfo = (state, action) => {
    return {
        ...state,
        userInfo: action.userInfo,
        authorised: true
    }
};

const logOut = () => {
    sessionStorage.clear();
    window.location.href = '/';

}

const saveArticles = (state, action) => {
    return {
        ...state,
        articles: action.payload
    }
};

const deleteArticle = (state, action) => {
    let filteredArticles = state.articles.filter(x => x.id !== action.payload);
    return {
        ...state,
        articles: filteredArticles
    }
};

const publishArticles = (state, action) => {
    return {
        ...state,
        editForm: null,
        articles: [...state.articles, action.payload]
    }
};


const toggleArticleLike = (state, action) => {
    return {
        ...state,
        articles: state.articles.map((t) =>
            // idx === (action.payload.id - 1) ? { ...t, Like: !t.Like } : t
            t.id === (action.payload.id) ? { ...t, isLiked: !t.isLiked } : t
        )
    };
}

const setupDetailsView = (state, action) => {
    return {
        ...state,
        editForm: action.payload
    };
}

const clearArticle = (state, action) => {
    return {
        ...state,
        editForm: null
    };
}

const updateArticle = (state, action) => {
    const index = state.articles.findIndex(x => x.id === action.payload.id);
    let updatedArticle = [...state.articles];
    updatedArticle[index] = action.payload;

    return {
        ...state,
        editForm: null,
        articles: updatedArticle
    }
}


export const reducer = (state, action) => {
    switch (action.type) {
        case actionTypes.USER_LOGIN: return saveUserInfo(state, action);
        case actionTypes.LOAD_ARTICLES: return saveArticles(state, action);
        case actionTypes.DELETE_ARTICLE: return deleteArticle(state, action);
        case actionTypes.TOGGLE_LIKE: return toggleArticleLike(state, action);
        case actionTypes.SETUP_DETAILS_VIEW: return setupDetailsView(state, action);
        case actionTypes.PUBLISH_ARTICLE: return publishArticles(state, action);
        case actionTypes.LOGOUT: return logOut();
        case actionTypes.CLEAR_ARTICLE: return clearArticle(state, action);
        case actionTypes.UPDATE_ARTICLE: return updateArticle(state, action);

        default: return state;
    }



}