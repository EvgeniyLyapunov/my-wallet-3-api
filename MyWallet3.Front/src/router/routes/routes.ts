import { RouteRecordRaw } from 'vue-router';

const routes: RouteRecordRaw[] = [
  {
    path: '/',
    name: 'home', 
    component: () => import('@/pages/index.vue'),
    meta: {
      needAuth: false, 
      withAppLayout: true,
    }
  },
  {
    path: '/:catchAll(.*)*',
    name: 'notFound',
    component: () => import('@/pages/[...path].vue'),
    meta: {
      needAuth: false, 
      withAppLayout: false,
    }
  },
];

export default routes;