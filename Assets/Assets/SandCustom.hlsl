#ifndef SANDCUSTOM_INCLUDED
#define SANDCUSTOM_INCLUDED


float squareComponenet(float a, float b, float c){
    return (a-b) * (a-b) * c;
}

void Looper_float(float3 trailMaker, float3 makerVelocity, float3 myWorld, float iterations, float scaler, float height, float max, float extra, float length, float fade, out float3 Displace){

    float3 tempDisplace = (0.0,0.0,0.0);
    float parabolapartA = 0;
    float parabolapartB = 0;
    float3 tempTrail = (0.0,0.0,0.0);

    float points[64];

    for(int i = 1; i<=iterations; i++){
        tempTrail = trailMaker - (makerVelocity * i *length);
        //tempTrail += float3(0,0,0);
        parabolapartB = extra * tempTrail.y;
        parabolapartA = (squareComponenet(myWorld.x,tempTrail.x,scaler)+squareComponenet(myWorld.z,tempTrail.z,scaler));

        tempDisplace += clamp((parabolapartA - parabolapartB)+(height+fade*(i/iterations)),0,max);
    }

    Displace = tempDisplace;
}

#endif