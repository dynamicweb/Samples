export default class ExpressDeliveryWidget extends HTMLElement {
    connectedCallback() {
        console.log("ExpressDeliveryWidget connected")
    }

    disconnectedCallback() {
    }
}

customElements.define("express-delivery-widget", ExpressDeliveryWidget);