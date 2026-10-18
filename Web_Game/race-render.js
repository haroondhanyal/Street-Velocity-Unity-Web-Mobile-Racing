(() => {
  const car = new Image();
  car.src = 'assets/street-car.png';
  window.drawStreetRace = (now, state) => {
    const canvas = document.querySelector('.screen.active #raceCanvas');
    if (!canvas) return;
    const bounds = canvas.getBoundingClientRect();
    const qualityScale = state.quality === 'Low' ? 0.75 : state.quality === 'Medium' ? 1 : 1.25;
    const ratio = Math.min((window.devicePixelRatio || 1) * qualityScale, 2);
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
    ctx.strokeStyle = state.settings?.vfx === false ? 'rgba(93,128,150,.5)' : 'rgba(47,218,255,.72)';
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

    const lapLength = state.trackLength || 1250;
    const lapProgress = state.distance % lapLength;
    const checkpointDistance = lapLength / 6 - (lapProgress % (lapLength / 6));
    const gateT = 1 - checkpointDistance / 520;
    if (gateT > 0 && gateT < 0.96 && state.settings?.vfx !== false) {
      const y = horizon + (height - horizon) * gateT * gateT;
      const half = topHalf + (bottomHalf - topHalf) * gateT;
      const postH = 12 + gateT * 42;
      ctx.save();
      ctx.shadowColor = '#25dcff'; ctx.shadowBlur = 18 * gateT;
      ctx.strokeStyle = `rgba(36,220,255,${0.35 + gateT * 0.6})`;
      ctx.lineWidth = 1 + gateT * 5;
      ctx.beginPath();
      ctx.moveTo(center - half * 0.92, y); ctx.lineTo(center - half * 0.92, y - postH);
      ctx.lineTo(center + half * 0.92, y - postH); ctx.lineTo(center + half * 0.92, y);
      ctx.stroke(); ctx.restore();
    }

    for (const [index, opponent] of (state.opponents || []).entries()) {
      const gap = opponent.distance - state.distance;
      if (gap < -90 || gap > 620) continue;
      const t = Math.max(0.06, Math.min(0.98, 1 - gap / 700));
      const y = horizon + (height - horizon) * t * t;
      const half = topHalf + (bottomHalf - topHalf) * t;
      const x = center + opponent.lane * half * 0.72;
      const w = 5 + t * 34;
      const h = 7 + t * 52;
      const color = index % 2 ? '#22d9ff' : '#ff285f';
      ctx.fillStyle = '#03070d';
      ctx.fillRect(x - w * 0.55, y - h * 0.78, w * 1.1, h);
      ctx.fillStyle = color;
      ctx.fillRect(x - w * 0.5, y - h * 0.18, w * 0.2, Math.max(2, h * 0.08));
      ctx.fillRect(x + w * 0.3, y - h * 0.18, w * 0.2, Math.max(2, h * 0.08));
    }

    if (car.complete && car.naturalWidth) {
      const carWidth = Math.min(width * 0.27, 290);
      const carHeight = carWidth * 0.67;
      const x = center + state.lane * width * 0.2;
      if (state.settings?.shadows !== false) { ctx.save(); ctx.shadowColor = '#000'; ctx.shadowBlur = 22; ctx.shadowOffsetY = 12; }
      ctx.drawImage(car, x - carWidth / 2, height - carHeight * 0.93, carWidth, carHeight);
      if (state.settings?.shadows !== false) ctx.restore();
    }

    if (state.settings?.postProcessing !== false) {
      const glow = ctx.createLinearGradient(0, horizon, 0, height);
      glow.addColorStop(0, 'rgba(0,206,255,0)');
      glow.addColorStop(1, state.weather === 'Clear (Day)' ? 'rgba(255,197,91,.08)' : 'rgba(0,206,255,.12)');
      ctx.fillStyle = glow;
      ctx.fillRect(0, horizon, width, height - horizon);
    }
    if (state.weather === 'Rain' || state.weather === 'Fog') {
      ctx.save();
      if (state.weather === 'Fog') {
        const fog = ctx.createLinearGradient(0, horizon, 0, height);
        fog.addColorStop(0, 'rgba(185,210,224,.24)'); fog.addColorStop(1, 'rgba(185,210,224,.03)');
        ctx.fillStyle = fog; ctx.fillRect(0, horizon, width, height - horizon);
      } else {
        ctx.strokeStyle = 'rgba(165,220,255,.25)'; ctx.lineWidth = 1;
        for (let i = 0; i < 55; i++) {
          const x = (i * 97 + now * 0.23) % width;
          const y = (i * 53 + now * 0.55) % height;
          ctx.beginPath(); ctx.moveTo(x, y); ctx.lineTo(x - 5, y + 15); ctx.stroke();
        }
      }
      ctx.restore();
    }
    void now;
  };
})();
