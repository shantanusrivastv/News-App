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


export const reducer = (state, action) => {
    switch ( action.type ) {
        case actionTypes.USER_LOGIN: return saveUserInfo( state, action );
        case actionTypes.LOAD_PUBLISHER_ARTICLES: return savePublisherArticles( state, action );
        default: return state;
    }



}