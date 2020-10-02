import axios from 'axios';
import {Configs} from './constants'

const getToken =() =>{
    if(sessionStorage.getItem("userInfo")){
        return JSON.parse(sessionStorage.getItem("userInfo")).token;
    }
    return "";
}

const instance = axios.create({
    baseURL: Configs.URL,
    headers: {
        'Content-Type' : 'application/json',
        'Accept' : 'application/json',
        'Authorization': 'Bearer '+ getToken()
    }
});

export default instance;
