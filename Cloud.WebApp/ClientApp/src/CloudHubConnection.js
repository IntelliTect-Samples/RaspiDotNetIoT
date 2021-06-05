import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr'

export default {
    install(Vue) {
        this.connection = new HubConnectionBuilder()
            .withUrl('http://localhost:50598/cloudhub')
            .withAutomaticReconnect()
            .configureLogging(LogLevel.Information)
            .build();

        async function start() {
            try {
                await this.connection.start();
                console.log("SignalR Connected.");
                connection.invoke('JoinWebAppClientGroup')

            } catch (err) {
                console.log(err);
                setTimeout(start, 5000);
            }
        }

        this.connection.onclose(start);

        this.connection.on

        // Start the connection.
        start();

        // use new Vue instance as an event bus
        const cloudHub = new Vue()
        // every component will use this.$cloudHub to access the event bus
        Vue.prototype.$cloudHub = cloudHub

        // Forward server side SignalR events through $questionHub, where components will listen to them
        connection.on('Degree_Status', (degree) => {
            cloudHub.$emit('degree-status', { degree })
        })

        // client to server
        cloudHub.joinWebAppClientGroup = () => {
            return connection.invoke('JoinWebAppClientGroup')
        }

        cloudHub.setDegree = (degree) => {
            return connection.invoke('SetDegree', degree)
        }
    }
}