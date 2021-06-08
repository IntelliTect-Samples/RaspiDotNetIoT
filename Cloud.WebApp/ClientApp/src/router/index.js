import { createWebHistory, createRouter } from "vue-router";
import ServoController from "@/components/ServoController.vue";

const routes = [
    {
        path: "/",
        name: "ServoController",
        component: ServoController,
    },
];

const router = createRouter({
    history: createWebHistory(),
    routes,
});

export default router;