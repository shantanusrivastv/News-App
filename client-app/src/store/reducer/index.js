import { actionTypes } from '../../Common/constants'

const saveUserInfo = (state, action) => {
    return {
        ...state,
        userInfo: action.userInfo,
        authorised: true
    }
};

const logOut = () =>{
    sessionStorage.clear();
    window.location.href = '/';
    
}

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
          t.id === (action.payload.id) ? { ...t, isLiked: !t.isLiked } : t
          // t.id === (action.payload.id) ? {isLiked: !t.isLiked } : t
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

const updateArticle = (state, action)=>{
        let oArticles = [...state.publisherArticles];
        for (let article of oArticles) {
            article = (article.Id === action.payload.Id) ? action.payload : article //Update Article
        }
        return {
            ...state,
            publisherArticles: oArticles,
            editForm: null
        }
    }


export const reducer = (state, action) => {
    switch (action.type) {
        case actionTypes.USER_LOGIN: return saveUserInfo(state, action);
        case actionTypes.LOAD_PUBLISHER_ARTICLES: return savePublisherArticles(state, action);
        case actionTypes.TOGGLE_LIKE: return toggleArticleLike(state, action);
        case actionTypes.SETUP_DETAILS_VIEW: return setupDetailsView(state, action);
        case actionTypes.PUBLISH_ARTICLE: return publishArticles(state, action);
        case actionTypes.LOGOUT: return logOut();
        case actionTypes.CLEAR_ARTICLE: return clearArticle(state, action);
        case actionTypes.UPDATE_ARTICLE: return updateArticle(state, action);

        default: return state;
    }



}