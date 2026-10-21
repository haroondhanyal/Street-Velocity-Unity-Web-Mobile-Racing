import * as THREE from 'three';

const box=(w,h,d,material)=>new THREE.Mesh(new THREE.BoxGeometry(w,h,d),material);
const sphere=(r,material,w=12,h=8)=>new THREE.Mesh(new THREE.SphereGeometry(r,w,h),material);

/** Procedural street cars share a common footprint so the simple driving physics
 * can handle every vehicle while the silhouettes and trim make traffic varied. */
export function createCar(color=0xd82050,headlights=true,style='coupe'){
  const root=new THREE.Group();root.name=style==='player'?'Retro Sport':`Velocity Bay ${style}`;
  const paintParts=[];const addPaint=(mesh)=>{mesh.userData.isCarPaint=true;paintParts.push(mesh);root.add(mesh);};
  const paint=new THREE.MeshStandardMaterial({color,metalness:.62,roughness:.29});
  const trim=new THREE.MeshStandardMaterial({color:0x11151e,metalness:.35,roughness:.3});
  const glass=new THREE.MeshStandardMaterial({color:0x263e50,metalness:.42,roughness:.2});
  const chrome=new THREE.MeshStandardMaterial({color:0xb7c4c8,metalness:.9,roughness:.22});
  const lamp=new THREE.MeshStandardMaterial({color:0xffe2ac,emissive:0xffb964,emissiveIntensity:2.2});
  const redLamp=new THREE.MeshStandardMaterial({color:0xff315b,emissive:0xff123c,emissiveIntensity:1.8});
  const width=style==='suv'?2.15:2.02;
  const body=box(width,.56,4.15,paint);body.position.y=.43;addPaint(body);

  const hood=box(width*.92,.2,1.18,paint);hood.position.set(0,.72,-1.35);addPaint(hood);
  const trunk=box(width*.88,.19,.92,paint);trunk.position.set(0,.69,1.49);addPaint(trunk);
  const cabin=box(style==='suv'?1.62:1.53,style==='suv'?.82:.68,style==='sedan'?2.05:1.78,glass);
  cabin.position.set(0,1.03,style==='sedan'?.02:.03);root.add(cabin);
  const roof=box(style==='suv'?1.48:1.32,.12,style==='sedan'?1.54:1.15,paint);roof.position.set(0,1.46,.09);addPaint(roof);
  // A dark dashboard and two seat backs show through the cabin glazing.
  const interior=box(1.28,.3,1.25,trim);interior.position.set(0,.91,.14);root.add(interior);
  for(const x of [-.42,.42]){const seat=box(.48,.36,.32,trim);seat.position.set(x,1.02,.46);root.add(seat);}

  // Side windows, door seams and handles break up the box silhouette.
  for(const side of [-1,1]){
    const window=box(.035,.38,style==='sedan'?.69:.62,glass);window.position.set(side*.78,1.11,.08);root.add(window);
    const rearWindow=box(.035,.34,.47,glass);rearWindow.position.set(side*.78,1.08,.74);root.add(rearWindow);
    const seam=box(.025,.37,.025,trim);seam.position.set(side*(width/2+.006),.74,.17);root.add(seam);
    const handle=box(.035,.045,.22,chrome);handle.position.set(side*(width/2+.018),.88,.26);root.add(handle);
    const mirror=new THREE.Group();mirror.position.set(side*.92,.99,-.52);const arm=box(.13,.055,.12,trim);mirror.add(arm);const cap=box(.16,.12,.2,paint);cap.position.x=side*.08;cap.userData.isCarPaint=true;paintParts.push(cap);mirror.add(cap);root.add(mirror);
  }

  const frontBar=box(1.72,.15,.12,trim);frontBar.position.set(0,.22,-2.1);root.add(frontBar);
  const backBar=box(1.78,.16,.12,trim);backBar.position.set(0,.2,2.1);root.add(backBar);
  const grille=box(.78,.2,.035,trim);grille.position.set(0,.43,-2.12);root.add(grille);
  for(let i=-2;i<=2;i++){
    const slat=box(.035,.14,.025,chrome);slat.position.set(i*.14,.43,-2.145);root.add(slat);
  }
  for(const x of [-.7,.7]){
    const head=box(.42,.15,.08,lamp);head.position.set(x,.49,-2.12);root.add(head);
    const tail=box(.45,.16,.08,redLamp);tail.position.set(x,.48,2.12);root.add(tail);
    const sideMarker=box(.04,.09,.22,lamp);sideMarker.position.set(x*1.42,.54,-1.48);root.add(sideMarker);
  }
  if(style==='taxi'){
    const sign=box(.55,.16,.3,new THREE.MeshStandardMaterial({color:0xffd24e,emissive:0xffb500,emissiveIntensity:1.1}));sign.position.set(0,1.57,.08);root.add(sign);
    const stripe=box(width+.025,.13,1.55,new THREE.MeshStandardMaterial({color:0xffd24e}));stripe.position.set(0,.67,.2);root.add(stripe);
  }
  if(style==='police'){
    for(const side of [-1,1]){const stripe=box(.035,.21,1.32,new THREE.MeshStandardMaterial({color:0x214a8d,metalness:.3,roughness:.4}));stripe.position.set(side*(width/2+.02),.62,.12);root.add(stripe);}
    const barBase=box(.68,.09,.28,trim);barBase.position.set(0,1.56,.03);root.add(barBase);
    const red=new THREE.MeshStandardMaterial({color:0xff304b,emissive:0xd90022,emissiveIntensity:2});
    const blue=new THREE.MeshStandardMaterial({color:0x36aaff,emissive:0x0874ff,emissiveIntensity:2});
    const redLight=box(.23,.13,.25,red);redLight.position.set(-.21,1.68,.03);root.add(redLight);
    const blueLight=box(.23,.13,.25,blue);blueLight.position.set(.21,1.68,.03);root.add(blueLight);
    root.userData.sirenLights=[redLight,blueLight];
  }
  if(style==='suv'){
    for(const x of [-.63,.63]){const rack=box(.08,.09,1.58,chrome);rack.position.set(x,1.56,.08);root.add(rack);}
    const crossbar=box(1.3,.07,.08,chrome);crossbar.position.set(0,1.57,.08);root.add(crossbar);
  }
  if(style==='van'){
    const sidePanel=box(.03,.5,.9,new THREE.MeshStandardMaterial({color:0xf4d4a2}));sidePanel.position.set(width/2+.015,.78,.45);root.add(sidePanel);
  }
  if(style==='sports'||style==='coupe'){
    const spoiler=box(1.7,.09,.2,trim);spoiler.position.set(0,.91,1.88);root.add(spoiler);
    for(const x of [-.62,.62]){const support=box(.08,.22,.09,trim);support.position.set(x,.8,1.83);root.add(support);}
  }

  const wheels=[];const tireMat=new THREE.MeshStandardMaterial({color:0x101116,roughness:.96});
  const rimMat=new THREE.MeshStandardMaterial({color:0xb8c4ca,metalness:.9,roughness:.25});
  const hubMat=new THREE.MeshStandardMaterial({color:0x333d48,metalness:.82,roughness:.3});
  for(const x of [-1,1])for(const z of [-1.35,1.35]){
    const wheel=new THREE.Group();wheel.position.set(x,.34,z);
    const tire=new THREE.Mesh(new THREE.CylinderGeometry(.39,.39,.24,20),tireMat);tire.rotation.z=Math.PI/2;wheel.add(tire);
    const rim=new THREE.Mesh(new THREE.CylinderGeometry(.255,.255,.25,16),rimMat);rim.rotation.z=Math.PI/2;wheel.add(rim);
    const hub=new THREE.Mesh(new THREE.CylinderGeometry(.105,.105,.26,12),hubMat);hub.rotation.z=Math.PI/2;wheel.add(hub);
    for(let spoke=0;spoke<5;spoke++){const bar=box(.035,.22,.035,chrome);bar.position.set(0,Math.sin(spoke*Math.PI*2/5)*.115,Math.cos(spoke*Math.PI*2/5)*.115);bar.rotation.x=spoke*Math.PI*2/5;wheel.add(bar);}
    root.add(wheel);wheels.push(wheel);
  }
  if(headlights)for(const x of [-.68,.68]){const beam=new THREE.SpotLight(0xe6f5ff,5,34,.3,.55,1.8);beam.position.set(x,.68,-2.05);beam.target.position.set(x,.1,-24);root.add(beam,beam.target);}
  root.userData.wheels=wheels;root.userData.paint=paint;root.userData.paintParts=paintParts;root.userData.style=style;return root;
}

export function createPerson(shirt=0x11a8a2,pants=0x263950,style=0){
  const root=new THREE.Group();
  const mat=(color,roughness=.84,metalness=0)=>new THREE.MeshStandardMaterial({color,roughness,metalness});
  const skin=mat([0xb97552,0xd59a70,0x8e5c43,0xe2b18b][style%4]);
  const top=mat(shirt),leg=mat(pants),hair=mat([0x211b20,0x38231d,0x171b22,0x594334][style%4]);
  const shoe=mat(style%3===0?0x171821:0xe7e0d2,.72);
  const torso=new THREE.Mesh(new THREE.CapsuleGeometry(.34,.54,5,10),top);torso.position.y=1.14;root.add(torso);
  const hem=new THREE.Mesh(new THREE.CylinderGeometry(.3,.34,.16,10),top);hem.position.y=.82;root.add(hem);
  const head=sphere(.235,skin,16,12);head.position.y=1.87;root.add(head);
  const hairCap=sphere(.241,hair,14,10);hairCap.position.y=1.94;root.add(hairCap);
  // Face details point down the character's forward (-Z) direction.
  for(const x of [-.082,.082]){const eye=sphere(.025,mat(0x191820,.4),8,6);eye.position.set(x,1.88,-.218);root.add(eye);}
  const nose=new THREE.Mesh(new THREE.ConeGeometry(.038,.11,6),skin);nose.rotation.x=-Math.PI/2;nose.position.set(0,1.82,-.244);root.add(nose);
  const collar=new THREE.Mesh(new THREE.CylinderGeometry(.13,.18,.12,8),top);collar.position.y=1.51;root.add(collar);
  const belt=new THREE.Mesh(new THREE.CylinderGeometry(.285,.285,.08,10),mat(0x28252b));belt.position.y=.78;root.add(belt);
  const limbs=[];
  for(const side of [-1,1]){
    const arm=new THREE.Mesh(new THREE.CapsuleGeometry(.105,.48,4,8),top);arm.position.set(side*.41,1.16,0);arm.rotation.z=side*-.16;root.add(arm);limbs.push(arm);
    const legMesh=new THREE.Mesh(new THREE.CapsuleGeometry(.13,.46,4,8),leg);legMesh.position.set(side*.17,.47,0);root.add(legMesh);limbs.push(legMesh);
    const foot=box(.22,.13,.37,shoe);foot.position.set(side*.17,.11,-.065);root.add(foot);
    const hand=sphere(.1,skin,8,6);hand.position.set(side*.42,.85,0);root.add(hand);
  }
  if(style%4===1){const backpack=box(.42,.48,.2,mat(0x3a3941));backpack.position.set(0,1.13,.29);root.add(backpack);}
  if(style%4===2){const brim=new THREE.Mesh(new THREE.CylinderGeometry(.24,.24,.045,12),hair);brim.position.y=2.08;root.add(brim);}
  root.userData.limbs=limbs;root.userData.walkPhase=style*.7;return root;
}
