(() => {
  const car = new Image();
  car.src = 'assets/street-car.png';
  window.drawStreetRace = (now, state) => {
    const canvas = document.querySelector('.screen.active #raceCanvas');
    if (!canvas) return;
    const bounds = canvas.getBoundingClientRect();
    const ratio = Math.min(window.devicePixelRatio || 1, 2);
    const width = bounds.width;
    const height = bounds.height;
    if (canvas.width !== Math.round(width * ratio) || canvas.height !== Math.round(height * ratio)) {
      canvas.width = Math.round(width * ratio);
      canvas.height = Math.round(height * ratio);
    }
    const ctx = canvas.getContext('2d');
    ctx.setTransform(ratio, 0, 0, ratio, 0, 0);
    ctx.clearRect(0, 0, width, height);

    const horizon = height * 0.39;
    const drift = Math.sin(state.distance * 0.012) * width * 0.065;
    const center = width * 0.5 + drift;
    const topHalf = width * 0.105;
    const bottomHalf = width * 0.49;
    const phase = (state.distance * 0.075) % 1;

    const road = ctx.createLinearGradient(0, horizon, 0, height);
    road.addColorStop(0, '#344154');
    road.addColorStop(0.4, '#182334');
    road.addColorStop(1, '#080d15');
    ctx.fillStyle = road;
    ctx.beginPath();
    ctx.moveTo(center - topHalf, horizon);
    ctx.lineTo(center + topHalf, horizon);
    ctx.lineTo(center + bottomHalf, height);
    ctx.lineTo(center - bottomHalf, height);
    ctx.closePath();
    ctx.fill();

    ctx.lineWidth = 2;
    ctx.strokeStyle = 'rgba(47,218,255,.72)';
    ctx.beginPath();
    ctx.moveTo(center - topHalf, horizon);
    ctx.lineTo(center - bottomHalf, height);
    ctx.moveTo(center + topHalf, horizon);
    ctx.lineTo(center + bottomHalf, height);
    ctx.stroke();

    for (let lane = 1; lane <= 2; lane++) {
      for (let i = 0; i < 17; i++) {
        const t = (i / 17 + phase) % 1;
        const y = horizon + (height - horizon) * t * t;
        const half = topHalf + (bottomHalf - topHalf) * t;
        const dash = 2 + t * 23;
        const x = center - half + (2 * half * lane / 3);
        ctx.fillStyle = 'rgba(228,244,255,.7)';
        ctx.fillRect(x - dash * 0.035, y, Math.max(2, dash * 0.09), dash);
      }
    }

    for (let i = 0; i < 6; i++) {
      const t = (i * 0.2 + phase * 0.55) % 1;
      const y = horizon + (height - horizon) * t * t;
      const half = topHalf + (bottomHalf - topHalf) * t;
      const x = center + [-0.56, 0.48, -0.08, 0.72, -0.76, 0.16][i] * half * 0.72;
      const w = 3 + t * 28;
      const h = 5 + t * 43;
      ctx.fillStyle = '#03070d';
      ctx.fillRect(x - w * 0.55, y - h * 0.75, w * 1.1, h);
      ctx.fillStyle = i % 2 ? '#20d9ff' : '#ff285f';
      ctx.fillRect(x - w * 0.5, y - h * 0.18, w * 0.2, Math.max(2, h * 0.08));
      ctx.fillRect(x + w * 0.3, y - h * 0.18, w * 0.2, Math.max(2, h * 0.08));
    }

    if (car.complete && car.naturalWidth) {
      const carWidth = Math.min(width * 0.27, 290);
      const carHeight = carWidth * 0.67;
      const x = center + state.lane * width * 0.2;
      ctx.drawImage(car, x - carWidth / 2, height - carHeight * 0.93, carWidth, carHeight);
    }

    const glow = ctx.createLinearGradient(0, horizon, 0, height);
    glow.addColorStop(0, 'rgba(0,206,255,0)');
    glow.addColorStop(1, 'rgba(0,206,255,.12)');
    ctx.fillStyle = glow;
    ctx.fillRect(0, horizon, width, height - horizon);
    void now;
  };
})();
