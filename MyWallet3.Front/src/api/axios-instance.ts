import axios from 'axios';

export const customAxiosInstance = axios.create({
});

customAxiosInstance.interceptors.request.use(
  (config) => {
    // TODO: Позже мы добавим сюда логику получения токена из Local Storage
    // const token = localStorage.getItem('access_token');
    // if (token) {
    //   config.headers.Authorization = `Bearer ${token}`;
    // }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

customAxiosInstance.interceptors.response.use(
  (response) => {
    return response;
  },
  (error) => {
    // TODO: Позже мы добавим сюда логику автоматического обновления токена (refresh token),
    // если сервер вернул ошибку 401 (Unauthorized).
    return Promise.reject(error);
  }
);