import axios from 'axios';
import {Configs} from './constants'

const token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IkFkbWluIFVzZXIiLCJyb2xlIjoiUHVibGlzaGVyIiwibmFtZWlkIjoiYWRtaW5Vc2VyQHByZXNzZm9yZC5jb20iLCJuYmYiOjE2MDE1NTQ1MTUsImV4cCI6MTYwMjE1OTMxNSwiaWF0IjoxNjAxNTU0NTE1fQ.prFftzzj8nAFWllvoVnromwFuiJOn_X85n479DD8E-s";

const instance = axios.create({
    baseURL: Configs.URL,
    headers: {'Authorization': 'Bearer '+token}
});

export default instance;
