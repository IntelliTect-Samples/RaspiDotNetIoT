<template>
    
<div>
    <p>This component demonstrates real-time iot.</p>

    <p v-if="!this.currentDegree"><em>Loading... NotConnected...</em></p>

    <h1>Servo Controller</h1>

    <div class="badge badge-info" style="position: relative; padding: 10px 2px 10px 2px"> 
         <div class="card-body">
              <h6 class="card-title">Current Angle</h6>
                <h4>{{ currentDegree }}</h4>
         </div>
     </div>
    

  <div style="padding: 10px 10px 10px 10px"> 
         <div class="card-body">
             <h5 class="card-title">Enter Angle:</h5>
             <input v-model="degreeInput" type="text" placeholder="0-180">
             <button class="btn btn-primary" @click="setDegree(degreeInput)">Set</button>
         </div>
     </div>
     
     <div style="padding: 10px 10px 10px 10px"> 
         <div class="card-body">
             <h5 class="card-title">Controller</h5>
             <input type="range" class="form-control-range" id="formControlRange" min="0" max="180" v-model="sliderInputDegree">
         </div>
     </div>

    </div>
</template>


<script>
    
    
    export default {
        name: "ServoController",
        data() {
            return {
                degreeInput: 0,
                currentDegree: 0,
                sliderInputDegree: 0
            }
        },
        watch: {
            sliderInputDegree: function (newVal) {
                if (newVal != this.currentDegree){
                    this.setDegree(newVal)
                }
            },
            currentDegree: function(newVal) {this.sliderInputDegree = newVal }
        },
        
        created() {
            // Listen to "degree changes" coming from SignalR events that are on the Vue event bus now
            this.$emitter.on('degree-status', this.degreeChanged)
            
        },
        methods: {
            async setDegree(degree) {
                try {
                    this.$emitter.emit('setDegree',parseInt(degree));
                } catch (err) { 
                    console.error(err);
                }
            },
            degreeChanged(newDegree) {
                this.currentDegree = newDegree;
                
            }
        },
        mounted() {
           

           
        },
        beforeUnmount() {
            // Make sure to cleanup SignalR event handlers when removing the component
            this.$emitter.off('degree-status', this.setDegree)
        },
    }
</script>