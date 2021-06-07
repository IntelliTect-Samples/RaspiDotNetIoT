<template>
  <div>
    <p>This component demonstrates real-time iot.</p>

    <p v-if="this.currentDegree == -1"><em>Loading... NotConnected...</em></p>

    <h1>Servo Controller</h1>

    <div class="clockBody">
      <div class="clock">
        <div class="wrap">
          <span class="minute"></span>
          <span class="dot"></span>
          <span style="top: 65%; position: relative;">
            <h6>Current Angle</h6>
            <h4>{{ currentDegree }}</h4>
          </span>
        </div>
      </div>
    </div>

    <div style="padding: 10px 10px 10px 10px">
      <div class="card-body">
        <h5 class="card-title">Enter Angle:</h5>
        <input v-model="degreeInput" type="text" placeholder="0-150" />
        <button class="btn btn-primary" @click="setDegree(degreeInput)">
          Set
        </button>
      </div>
    </div>

    <div style="padding: 10px 10px 10px 10px">
      <div class="card-body">
        <h5 class="card-title">Controller</h5>
        <input
          type="range"
          class="form-control-range"
          id="formControlRange"
          min="0"
          max="150"
          v-model="sliderInputDegree"
        />
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
      currentDegree: -1,
      sliderInputDegree: 0,
    };
  },
  watch: {
    sliderInputDegree: function(newVal) {
      if (newVal != this.currentDegree) {
        this.setDegree(newVal);
      }
    },
    currentDegree: function(newVal) {
      this.sliderInputDegree = newVal;
    },
  },

  created() {
    // Listen to "degree changes" coming from SignalR events that are on the Vue event bus now
    this.$emitter.on("degree-status", this.degreeChanged);
    this.$emitter.emit("get-degree");
  },
  methods: {
    async setDegree(degree) {
      try {
        this.$emitter.emit("set-degree", parseInt(degree));
      } catch (err) {
        console.error(err);
      }
    },
    degreeChanged(newDegree) {
      this.currentDegree = newDegree;
      this.clock(newDegree);
    },
    clock(degree) {
   
      document.querySelector(".minute").style.transform = `rotate(${degree -
        90}deg)`;
    },
  },
  beforeUnmount() {
    // Make sure to cleanup SignalR event handlers when removing the component
    this.$emitter.off("degree-status", this.setDegree);
  },
};
</script>

<style scoped>
.clockBody {
  display: flex;
  align-items: center;
  justify-content: center;
}

.clock {
  border-radius: 100%;
  background: #ffffff;
  font-family: "Montserrat";
  border: 5px solid white;
  box-shadow: inset 2px 3px 8px 0 rgba(0, 0, 0, 0.1);
}

.wrap {
  overflow: hidden;
  position: relative;
  width: 350px;
  height: 350px;
  border-radius: 100%;
}

.minute,
.hour {
  position: absolute;
  height: 100px;
  width: 6px;
  margin: auto;
  top: -27%;
  left: 0;
  bottom: 0;
  right: 0;
  background: black;
  transform-origin: bottom center;
  transform: rotate(0deg);
  box-shadow: 0 0 10px 0 rgba(0, 0, 0, 0.4);
  z-index: 1;
}

.minute {
  position: absolute;
  height: 130px;
  width: 4px;
  top: -38%;
  left: 0;
  box-shadow: 0 0 10px 0 rgba(0, 0, 0, 0.4);
  transform: rotate(90deg);
}

.second {
  position: absolute;
  height: 90px;
  width: 2px;
  margin: auto;
  top: -26%;
  left: 0;
  bottom: 0;
  right: 0;
  border-radius: 4px;
  background: #ff4b3e;
  transform-origin: bottom center;
  transform: rotate(180deg);
  z-index: 1;
}

.dot {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  width: 12px;
  height: 12px;
  border-radius: 100px;
  background: white;
  border: 2px solid #1b1b1b;
  border-radius: 100px;
  margin: auto;
  z-index: 1;
}
</style>
