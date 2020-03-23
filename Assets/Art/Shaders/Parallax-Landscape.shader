// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Created/Parallax/Landscape"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_Back("Back", 2D) = "white" {}
		_Mid("Mid", 2D) = "white" {}
		_Color("Color", Color) = (0,0,0,0)
		_Alphastarts("Alpha starts", Range( 0 , 1)) = 0
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) fixed3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float2 texcoord_0;
			float3 viewDir;
			INTERNAL_DATA
			float2 texcoord_1;
		};

		uniform float4 _Color;
		uniform sampler2D _Back;
		uniform sampler2D _Mid;
		uniform float _Alphastarts;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			o.texcoord_0.xy = v.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
			o.texcoord_1.xy = v.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 Offset7 = ( ( 0.0 - 1.0 ) * i.viewDir.xy * 0.2915556 ) + i.texcoord_0;
			float2 Offset2 = ( ( 0.0 - 1.0 ) * i.viewDir.xy * 0.03827263 ) + i.texcoord_1;
			float4 tex2DNode6 = tex2D( _Mid, Offset2 );
			float4 lerpResult11 = lerp( tex2D( _Back, Offset7 ) , tex2DNode6 , ( tex2DNode6.b * _Alphastarts ));
			float4 blendOpSrc26 = _Color;
			float4 blendOpDest26 = lerpResult11;
			o.Emission = ( saturate( ( 1.0 - ( ( 1.0 - blendOpDest26) / blendOpSrc26) ) )).rgb;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows vertex:vertexDataFunc 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			# include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float3 worldPos : TEXCOORD6;
				float4 tSpace0 : TEXCOORD1;
				float4 tSpace1 : TEXCOORD2;
				float4 tSpace2 : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v, customInputData );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				fixed3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				fixed tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				fixed3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			fixed4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				fixed3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.viewDir = worldViewDir;
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=13101
7;29;1352;692;1789.49;-342.563;1;True;True
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;15;-1240.483,894.7744;Float;False;World;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;16;-1472.706,785.4388;Float;False;Constant;_MidDistance;MidDistance;5;0;0.03827263;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.TextureCoordinatesNode;3;-1233.74,455.9469;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ParallaxMappingNode;2;-788.0259,646.2068;Float;False;Normal;4;0;FLOAT2;0,0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT3;0,0,0;False;1;FLOAT2
Node;AmplifyShaderEditor.RangedFloatNode;19;-1472.235,651.8499;Float;False;Constant;_BackDistance;BackDistance;5;0;0.2915556;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.TextureCoordinatesNode;39;-1241.263,325.0096;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;44;-583.0339,917.1514;Float;False;Property;_Alphastarts;Alpha starts;3;0;0;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.ParallaxMappingNode;7;-787.3255,454.4958;Float;False;Normal;4;0;FLOAT2;0,0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT3;0,0,0;False;1;FLOAT2
Node;AmplifyShaderEditor.SamplerNode;6;-478.0205,622.7692;Float;True;Property;_Mid;Mid;1;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;5;-478.0205,431.7691;Float;True;Property;_Back;Back;0;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;43;-149.7935,743.851;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.LerpOp;11;6.815712,558.1436;Float;False;3;0;COLOR;0.0;False;1;COLOR;0.0,0,0,0;False;2;FLOAT;0.0;False;1;COLOR
Node;AmplifyShaderEditor.ColorNode;25;210.8914,404.5844;Float;False;Property;_Color;Color;2;0;0,0,0,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.BlendOpsNode;26;434.5882,632.1509;Float;False;ColorBurn;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;825.5312,591.8245;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Created/Parallax/Landscape;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Opaque;0.5;True;True;0;False;Opaque;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;2;0;3;0
WireConnection;2;2;16;0
WireConnection;2;3;15;0
WireConnection;7;0;39;0
WireConnection;7;2;19;0
WireConnection;7;3;15;0
WireConnection;6;1;2;0
WireConnection;5;1;7;0
WireConnection;43;0;6;3
WireConnection;43;1;44;0
WireConnection;11;0;5;0
WireConnection;11;1;6;0
WireConnection;11;2;43;0
WireConnection;26;0;25;0
WireConnection;26;1;11;0
WireConnection;0;2;26;0
ASEEND*/
//CHKSM=957434EC946DBC59BEB7180B51B8522EB1ECBB23