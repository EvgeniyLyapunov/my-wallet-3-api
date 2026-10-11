import 'vue-router'; 

declare module 'vue-router' {
  interface RouteMeta {
    needAuth: boolean;
    withAppLayout: boolean;
  }
}