<template>
    

    <p>This component demonstrates real-time iot.</p>

    <p v-if="!forecasts"><em>Loading...</em></p>

    <h1>Servo Angle</h1>

    <p>Enter Angle</p>

    <input v-model="degree" type="text" placeholder="0-180">

    <button class="btn btn-primary" @click="sendDegree">Send</button>

    <p>CurrentDegree: {{ currentDegree }}</p>
</template>


<script>
    
    import signalR from '@microsoft/signalr'
    export default {
        name: "FetchData",
        data() {
            return {
                degree: 0,
                currentDegree: 0,
            /*eslint no-undef: "warn"*/
                connection: null
            }
        },
        methods: {
            async sendDegree() {
                try {
                    await this.connection.invoke("SetDegree", this.degree);
                } catch (err) {
                    console.error(err);
                }
            }
        },
        mounted() {
           

            this.connection = new signalR.HubConnectionBuilder()
                .withUrl("/cloudhub")
                .withAutomaticReconnect()
                .configureLogging(signalR.LogLevel.Information)
                .build();

            async function start() {
                try {
                    await this.connection.start();
                    console.log("SignalR Connected.");
                } catch (err) {
                    console.log(err);
                    setTimeout(start, 5000);
                }
            }

            

            this.connection.onclose(start);

            // Start the connection.
            start();
        }
    }
</script>