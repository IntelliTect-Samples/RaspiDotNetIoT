import 'bootstrap/dist/css/bootstrap.css'
import { createApp } from 'vue'
import App from './App.vue'
import CloudHub from './cloudHubConnection'
import router from './router'


const app = createApp(App);
app.use(CloudHub)
app.use(router)
app.mount('#app')
