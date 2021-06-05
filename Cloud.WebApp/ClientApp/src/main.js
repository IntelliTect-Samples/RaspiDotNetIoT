import 'bootstrap/dist/css/bootstrap.css'
import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import CloudHub from './CloudHubConnection'


Vue.use(CloudHub)
createApp(App).use(router).mount('#app')
