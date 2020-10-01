import axios from 'axios';
import {Configs} from './constants'

const instance = axios.create({
    baseURL: Configs.URL,
});

export default instance;
