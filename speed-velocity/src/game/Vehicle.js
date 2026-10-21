import * as THREE from 'three';
const box=(w,h,d,material)=>new THREE.Mesh(new THREE.BoxGeometry(w,h,d),material);

export function createCar(color=0xd82050,headlights=true){
  const root=new THREE.Group();root.name='Retro Sport';
  const paint=new THREE.MeshStandardMaterial({color,metalness:.68,roughness:.25});
  const dark=new THREE.MeshStandardMaterial({color:0x101523,metalness:.3,roughness:.25});
  const glass=new THREE.MeshStandardMaterial({color:0x27495b,metalness:.35,roughness:.18});
  const lamp=new THREE.MeshStandardMaterial({color:0xffd59d,emissive:0xffa24d,emissiveIntensity:3});
  root.add(box(2.05,.55,4.1,paint));
  const bonnet=box(1.9,.22,1.24,paint);bonnet.position.set(0,.3,-1.26);root.add(bonnet);
  const cabin=box(1.55,.62,1.78,glass);cabin.position.set(0,.72,.1);root.add(cabin);
  const roof=box(1.28,.12,1.12,dark);roof.position.set(0,1.05,.16);root.add(roof);
  const spoiler=box(1.8,.08,.17,dark);spoiler.position.set(0,.71,1.86);root.add(spoiler);
  const frontBar=box(1.7,.13,.12,dark);frontBar.position.set(0,.08,-2.08);root.add(frontBar);
  for(const x of [-.68,.68]){const head=box(.35,.13,.09,lamp);head.position.set(x,.12,-2.12);root.add(head);const tail=new THREE.MeshStandardMaterial({color:0xff315b,emissive:0xff123c,emissiveIntensity:2});const back=box(.44,.12,.09,tail);back.position.set(x,.1,2.1);root.add(back);}
  if(headlights)for(const x of [-.62,.62]){const beam=new THREE.SpotLight(0xe6f5ff,5,34,.3,.55,1.8);beam.position.set(x,.36,-2.05);beam.target.position.set(x,.1,-24);root.add(beam,beam.target);}
  const wheels=[];const tireMat=new THREE.MeshStandardMaterial({color:0x111318,roughness:.94});const rimMat=new THREE.MeshStandardMaterial({color:0xaab9c5,metalness:.9,roughness:.25});
  for(const x of [-1,1])for(const z of [-1.35,1.35]){const wheel=new THREE.Group();wheel.position.set(x,.0,z);const tire=new THREE.Mesh(new THREE.CylinderGeometry(.39,.39,.23,16),tireMat);tire.rotation.z=Math.PI/2;wheel.add(tire);const rim=new THREE.Mesh(new THREE.CylinderGeometry(.19,.19,.24,12),rimMat);rim.rotation.z=Math.PI/2;wheel.add(rim);root.add(wheel);wheels.push(wheel);}
  root.userData.wheels=wheels;root.userData.paint=paint;return root;
}

export function createPerson(shirt=0x11a8a2, pants=0x263950){
  const root=new THREE.Group();
  const mat=(color,roughness=.86)=>new THREE.MeshStandardMaterial({color,roughness});
  const skin=mat(0xb97552), top=mat(shirt), leg=mat(pants), hair=mat(0x211b20), shoe=mat(0x151821);
  const torso=new THREE.Mesh(new THREE.CapsuleGeometry(.34,.55,4,8),top);torso.position.y=1.12;root.add(torso);
  const head=new THREE.Mesh(new THREE.SphereGeometry(.23,12,10),skin);head.position.y=1.85;root.add(head);
  const hairCap=new THREE.Mesh(new THREE.SphereGeometry(.235,12,8,0,Math.PI*2,0,Math.PI*.55),hair);hairCap.position.y=1.91;root.add(hairCap);
  const limbs=[];
  for(const side of [-1,1]){const arm=new THREE.Mesh(new THREE.CapsuleGeometry(.105,.5,3,6),top);arm.position.set(side*.42,1.15,0);arm.rotation.z=side*-.18;root.add(arm);limbs.push(arm);const legMesh=new THREE.Mesh(new THREE.CapsuleGeometry(.13,.47,3,6),leg);legMesh.position.set(side*.17,.48,0);root.add(legMesh);const foot=new THREE.Mesh(new THREE.BoxGeometry(.22,.12,.36),shoe);foot.position.set(side*.17,.1,-.06);root.add(foot);limbs.push(legMesh);}
  root.userData.limbs=limbs;return root;
}
