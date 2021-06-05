<template>
    

    <p>This component demonstrates real-time iot.</p>

    <p v-if="!forecasts"><em>Loading...</em></p>

    <h1>Servo Angle</h1>

    <p>Enter Angle</p>

    <input v-model="degree" type="text" placeholder="0-180">

    <button class="btn btn-primary" @click="setDegree">Set</button>

    <p>CurrentDegree: {{ currentDegree }}</p>
</template>


<script>
    
    
    export default {
        name: "FetchData",
        data() {
            return {
                degree: 0,
                currentDegree: 0
            }
        },
        created() {
            // Listen to "degree changes" coming from SignalR events that are on the Vue event bus now
            this.$cloudHub.$on('degree-status', this.degreeChanged)
            
        },
        methods: {
            async setDegree() {
                try {
                    await this.$cloudHub.setDegree(this.degree);
                } catch (err) {
                    console.error(err);
                }
            },
            degreeChanged({ newDegree }) {
                this.degree = newDegree;
            }
        },
        mounted() {
           

           
        },
        beforeDestroy() {
            // Make sure to cleanup SignalR event handlers when removing the component
            this.$cloudHub.$off('degree-status', this.onScoreChanged)
        },
    }
</script>