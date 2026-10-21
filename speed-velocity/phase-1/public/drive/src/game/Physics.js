export class VehiclePhysics {
  constructor() { this.speed = 0; this.heading = 0; this.condition = 100; }
  step(input, dt) {
    const throttle = input.forward ? 1 : 0;
    const brake = input.reverse ? 1 : 0;
    this.speed += throttle * 16 * dt;
    this.speed -= brake * (this.speed > 1 ? 28 : 10) * dt;
    this.speed -= this.speed * (input.handbrake ? 2.5 : 0.48) * dt;
    this.speed = Math.max(-9, Math.min(36, this.speed));
    const steerFactor = Math.min(1, Math.abs(this.speed) / 5);
    this.heading += input.steer * steerFactor * (input.handbrake ? 1.9 : 1.2) * dt * Math.sign(this.speed || 1);
    return {dx:Math.sin(this.heading) * this.speed * dt, dz:-Math.cos(this.heading) * this.speed * dt};
  }
}
