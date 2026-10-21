import * as THREE from 'three';
import {createCar,createPerson} from './Vehicle.js';

const mat=(color,roughness=.85,metalness=0,emissive=0)=>new THREE.MeshStandardMaterial({color,roughness,metalness,emissive,emissiveIntensity:emissive?1.5:0});
const cube=(w,h,d,m)=>new THREE.Mesh(new THREE.BoxGeometry(w,h,d),m);
const seeded=(n)=>{const x=Math.sin(n*127.1+311.7)*43758.5453;return x-Math.floor(x);};

function makeSky(){
  const geometry=new THREE.SphereGeometry(360,32,20);
  const material=new THREE.ShaderMaterial({side:THREE.BackSide,uniforms:{uPhase:{value:0.12}},vertexShader:'varying float h; varying vec3 dir; void main(){dir=normalize(position);h=dir.y;gl_Position=projectionMatrix*modelViewMatrix*vec4(position,1.0);}',fragmentShader:'uniform float uPhase; varying float h; varying vec3 dir; void main(){vec3 night=vec3(0.035,0.055,0.14);vec3 dusk=vec3(0.97,0.30,0.40);vec3 horizon=vec3(1.0,0.66,0.43);float sun=exp(-length(dir-vec3(0.15,0.16,-0.97))*13.0);vec3 sky=mix(night,dusk,smoothstep(-0.16,0.25,h));sky=mix(sky,horizon,smoothstep(-0.12,0.04,h));sky+=sun*vec3(1.0,0.37,0.13);sky=mix(sky,vec3(0.22,0.50,0.77),uPhase*smoothstep(0.0,0.65,h));gl_FragColor=vec4(sky,1.0);}'});
  return new THREE.Mesh(geometry,material);
}

function makeWindowTexture(seed,lit=true){
  const canvas=document.createElement('canvas');canvas.width=128;canvas.height=256;const c=canvas.getContext('2d');
  c.fillStyle=['#273849','#37404b','#364051','#59616b'][seed%4];c.fillRect(0,0,128,256);
  for(let y=12;y<250;y+=27)for(let x=10;x<124;x+=24){const glow=seeded(seed+x+y)>.38;c.fillStyle=glow?(seed%2?'#ffbc8c':'#7beeff'):'#142135';c.fillRect(x,y,12,17);if(glow){c.fillStyle='#fff0d2';c.fillRect(x+2,y+2,3,4);}}
  const texture=new THREE.CanvasTexture(canvas);texture.colorSpace=THREE.SRGBColorSpace;return new THREE.MeshStandardMaterial({map:texture,roughness:.64,metalness:.12,emissive:lit?0x152233:0x000000,emissiveIntensity:.4});
}

function addPalm(scene,x,z,scale=1){
  const palm=new THREE.Group();palm.position.set(x,0,z);palm.scale.setScalar(scale);
  const trunk=new THREE.Mesh(new THREE.CylinderGeometry(.25,.38,5.5,7),mat(0x77503a));trunk.position.y=2.75;trunk.rotation.z=.06;palm.add(trunk);
  const green=[0x174d45,0x1d6750,0x246d59];
  for(let i=0;i<9;i++){const leaf=new THREE.Mesh(new THREE.ConeGeometry(.42,4.4,5),mat(green[i%3]));const a=i*Math.PI*2/9;leaf.position.set(Math.sin(a)*1.5,5.5+Math.cos(a)*.15,Math.cos(a)*1.5);leaf.rotation.z=Math.PI/2-Math.cos(a)*.48;leaf.rotation.x=Math.sin(a)*.5;palm.add(leaf);}
  scene.add(palm);return palm;
}

function addStreetLight(scene,x,z,addLight=false){
  const group=new THREE.Group();group.position.set(x,0,z);const iron=mat(0x26303d,.38,.7);const pole=new THREE.Mesh(new THREE.CylinderGeometry(.08,.13,5.5,7),iron);pole.position.y=2.75;group.add(pole);
  const arm=cube(.13,.13,1.45,iron);arm.position.set(0,5.25,-.58);group.add(arm);
  const bulb=new THREE.Mesh(new THREE.BoxGeometry(.5,.16,.36),new THREE.MeshStandardMaterial({color:0xffd39a,emissive:0xffbd72,emissiveIntensity:3}));bulb.position.set(0,5.15,-1.12);group.add(bulb);
  if(addLight){const light=new THREE.PointLight(0xffc57b,7,18,2);light.position.copy(bulb.position);group.add(light);}scene.add(group);return group;
}

export function createWorld(scene){
  scene.background=new THREE.Color(0x171630);scene.fog=new THREE.Fog(0x261b3a,105,285);
  const sky=makeSky();scene.add(sky);
  scene.add(new THREE.HemisphereLight(0x95b9ff,0x433044,2.05));
  const sun=new THREE.DirectionalLight(0xffcb9b,3.1);sun.position.set(-65,100,-85);scene.add(sun);
  const floor=new THREE.Mesh(new THREE.PlaneGeometry(360,360),mat(0x4b5b50));floor.rotation.x=-Math.PI/2;floor.position.y=-.16;scene.add(floor);
  const sand=new THREE.Mesh(new THREE.PlaneGeometry(39,360),mat(0xd8b17c));sand.rotation.x=-Math.PI/2;sand.position.set(174,-.12,0);scene.add(sand);
  const oceanMat=new THREE.MeshStandardMaterial({color:0x167f99,roughness:.24,metalness:.15});const ocean=new THREE.Mesh(new THREE.PlaneGeometry(140,360),oceanMat);ocean.rotation.x=-Math.PI/2;ocean.position.set(263,-.22,0);scene.add(ocean);
  const concrete=mat(0xb29a87),asphalt=mat(0x292d39,.94),lane=mat(0xeec982),curb=mat(0xd8c3a6);
  const axes=[];for(let i=-4;i<=4;i++)axes.push(i*38);
  const dashVertical=new THREE.InstancedMesh(new THREE.BoxGeometry(.13,.025,4.2),lane,300),dashHorizontal=new THREE.InstancedMesh(new THREE.BoxGeometry(4.2,.03,.13),lane,300),dummy=new THREE.Object3D();let vi=0,hi=0;
  // A compact 12-district style grid connected by boulevards and side streets.
  for(const x of axes){const road=cube(12,.12,330,asphalt);road.position.set(x,-.02,0);scene.add(road);const path=cube(1.3,.1,330,concrete);path.position.set(x+7,-.02,0);scene.add(path);const path2=cube(1.3,.1,330,concrete);path2.position.set(x-7,-.02,0);scene.add(path2);
    for(let z=-160;z<165;z+=11){dummy.position.set(x,-.015,z);dummy.updateMatrix();dashVertical.setMatrixAt(vi++,dummy.matrix);}
  }
  for(const z of axes){const road=cube(330,.13,12,asphalt);road.position.set(0,-.01,z);scene.add(road);const path=cube(330,.1,1.3,concrete);path.position.set(0,-.015,z+7);scene.add(path);const path2=cube(330,.1,1.3,concrete);path2.position.set(0,-.015,z-7);scene.add(path2);
    for(let x=-160;x<165;x+=11){dummy.position.set(x,-.005,z);dummy.updateMatrix();dashHorizontal.setMatrixAt(hi++,dummy.matrix);}
  }
  dashVertical.count=vi;dashHorizontal.count=hi;dashVertical.instanceMatrix.needsUpdate=true;dashHorizontal.instanceMatrix.needsUpdate=true;scene.add(dashVertical,dashHorizontal);
  // Palm boulevard / beach promenade.
  const promenade=cube(3,.16,330,curb);promenade.position.set(156,.02,0);scene.add(promenade);
  for(let z=-150;z<=150;z+=25){addPalm(scene,160,z,1.05);addStreetLight(scene,144,z+10,z%75===0);}
  for(let z=-145;z<=145;z+=48){const lamp=addStreetLight(scene,8,z,z%96===0);lamp.rotation.y=Math.PI;}

  const blockers=[];const buildingMats=[0xd1a78a,0x9d9c9b,0x83aab0,0xf1c79f,0x6f8295,0xd68d82].map((c)=>mat(c,.72,.12));
  const windowMats=Array.from({length:6},(_,i)=>makeWindowTexture(i));
  for(let zi=0;zi<8;zi++)for(let xi=0;xi<8;xi++){
    const cx=-133+xi*38,cz=-133+zi*38;const r=seeded(xi*9+zi*23+4);const width=15+r*5,depth=14+seeded(zi*21+xi)*5,height=9+seeded(xi*33+zi*5)*28;
    const building=new THREE.Mesh(new THREE.BoxGeometry(width,height,depth),[buildingMats[Math.floor(r*buildingMats.length)],windowMats[Math.floor(r*windowMats.length)],buildingMats[2],windowMats[Math.floor(r*windowMats.length)],buildingMats[3],windowMats[Math.floor(r*windowMats.length)]]);
    building.position.set(cx,height/2,cz);building.castShadow=true;building.receiveShadow=true;scene.add(building);blockers.push({x:cx,z:cz,halfX:width*.52,halfZ:depth*.52});
    const roof=cube(width*.92,.34,depth*.92,mat(0x434653,.6,.25));roof.position.set(cx,height+.16,cz);scene.add(roof);
    if(r>.44){const sign=cube(width*.7,1.1,.17,new THREE.MeshStandardMaterial({color:0x231c34,emissive:r>.7?0xff4389:0x2ddaf2,emissiveIntensity:2.5}));sign.position.set(cx,height*.34,cz-depth*.51);scene.add(sign);}
    if(r>.77){const awning=cube(width*.68,.25,2.2,mat(0xeee1ca));awning.position.set(cx,.8,cz-depth*.51-1);scene.add(awning);}
  }
  // Art-deco hotel on the shoreline and a few storefronts.
  const hotel=cube(21,31,14,mat(0xf0c4a4));hotel.position.set(131,15.5,-15);scene.add(hotel);blockers.push({x:131,z:-15,halfX:11,halfZ:8});
  for(let floorNo=0;floorNo<8;floorNo++){const balcony=cube(22,.35,1,mat(0xf2dfc1));balcony.position.set(131,5+floorNo*3.5,-7.8);scene.add(balcony);}
  const hotelSign=cube(12,1.2,.2,new THREE.MeshStandardMaterial({color:0x271830,emissive:0xff4da2,emissiveIntensity:3}));hotelSign.position.set(131,4,-22.2);scene.add(hotelSign);

  const pedestrians=[];const shirts=[0x13a8a6,0xd84e58,0xe6b441,0x4963ac,0xec81b2,0xddd8c6];
  for(let i=0;i<10;i++){const person=createPerson(shirts[i%shirts.length],i%3?0x26354a:0x422d2c);const axis=axes[1+Math.floor(seeded(i+5)*7)];person.position.set(i%2?axis+8:axis,.02,-132+seeded(i+71)*264);person.userData.phase=seeded(i+19)*Math.PI*2;person.userData.speed=.35+seeded(i+32)*.4;person.userData.direction=seeded(i+13)*Math.PI*2;scene.add(person);pedestrians.push(person);}
  const traffic=[];const colors=[0x167fba,0xd9aa38,0x35a887,0xa64f79,0xead5b3,0x466273,0xe15b44];
  for(let i=0;i<6;i++){const car=createCar(colors[i%colors.length],false);const row=i%3;const z=axes[1+Math.floor(i/3)]+(row-1)*3;car.position.set(-130+i*45,.48,z);car.userData.trafficAxis=i%2?'x':'z';car.rotation.y=car.userData.trafficAxis==='x'?Math.PI/2:Math.PI;car.userData.trafficSpeed=7+seeded(i+8)*5;scene.add(car);traffic.push(car);}
  return {sky,blockers,pedestrians,traffic,sun,hotel};
}
