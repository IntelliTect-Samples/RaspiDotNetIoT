import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr'
import mitt from 'mitt';


export default {
    install(app) {

        const emitter = mitt()
        app.config.globalProperties.$emitter = emitter

        let hubUrl= 'https://nanuk-pro:45455/CloudHub' // put your url here.
        let connection = null
       

        connection = new HubConnectionBuilder()
            .withUrl(hubUrl)
            .withAutomaticReconnect()
            .configureLogging(LogLevel.Information)
            .build();

        connection.connected=false;

        async function start() {
            connection.connected=false;
            try {
                await connection.start();
                console.log("SignalR Connected.");
                connection.invoke('JoinWebAppClientGroup')
                connection.connected=true;

            } catch (err) {
                console.log(err);
                setTimeout(start, 5000);
            }
        }

        // Forward server side SignalR events through $questionHub, where components will listen to them
       connection.on('Degree_Status', (degree) => {
           emitter.emit('degree-status', degree )
        })

        console.log(this.$emitter)

        emitter.on('set-degree', degree => {           
            connection.invoke('SetDegree_WebApp', degree)
        })

        emitter.on('get-degree', ()=> {
           if(connection.connected)                
            connection.invoke('GetDegreeStatus')})

        connection.onclose(start);
        // Start the connection.
        start();


        
    }
}