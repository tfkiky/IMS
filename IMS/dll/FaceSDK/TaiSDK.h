

#define IN   
#define OUT


#define Tai_API(type) extern "C" _declspec(dllexport) type 




/*
��ʼ���� �����㷨��ʼ�������ݿ��ʼ��

����:
	dbname, ���ݿ�����

���:
	��

  ����: 
0   �ɹ�
-30 �����㷨��ʧ��
-40 ���ݿ��ʼ��ʧ��
  */
Tai_API(int) face_init();



/*
�ر����ݿ⣬�����㷨ж��

  */
Tai_API(int) face_exit();


/*
��ͼ���ȡ��������

����:
	pic_sec64, ǰ�˿ؼ��ɼ���ͼ���base64��
	savePic,   ͼ�񱣴��ļ����� 0��ʾ������


���:
	feature, ���base64�������������

  ����: 
>=0   �ɹ�
<0  ʧ��
-30 �����㷨��ʧ��
-40 ���ݿ��ʼ��ʧ��
-50 �㷨��δ��ʼ��
  */

Tai_API(int) face_get_feature(char *pic_sec64,char feature[3000],char *savePic);


/*
��ͼ���ȡ��������

����:
	fname,     ͼ���ļ���

���:
	feature, ���base64�������������

  ����: 
>=0   �ɹ�
<0  ʧ��
-30 �����㷨��ʧ��
-40 ���ݿ��ʼ��ʧ��
-50 �㷨��δ��ʼ��
  */
Tai_API(int) face_get_feature_from_image(char *fname,char feature[3000]);

/*
��ͼ���ȡ��������

����:
	pic_sec64, ǰ�˿ؼ��ɼ���ͼ���base64��
	savePic,   ͼ�񱣴��ļ����� 0��ʾ������


���:
	feature, ���base64�������������

  ����: 
>=0   �ɹ�
<0  ʧ��
-30 �����㷨��ʧ��
-40 ���ݿ��ʼ��ʧ��
-50 �㷨��δ��ʼ��
  */

Tai_API(int) face_get_feature(char *pic_sec64,char feature[3000],char *savePic);

/*
���������������жԱ�

����:
	feature1, ��������
	feature2, ��������


���:
	��

  ����: 
  �÷֣����127
>=0 �ɹ�
<0  ʧ��
-30 �����㷨��ʧ��
-40 ���ݿ��ʼ��ʧ��
-50 �㷨��δ��ʼ��
  */
Tai_API(int) face_comp_feature(char feature1[3000],char feature2[3000]);



/*
���������������жԱ�

����:
	feature1, ��������
	feature2, ��������


���:
	��

  ����: 
  �÷֣����127
>=0 �ɹ�
<0  ʧ��
-30 �����㷨��ʧ��
-40 ���ݿ��ʼ��ʧ��
-50 �㷨��δ��ʼ��
  */
typedef struct tagFace /* �ȳ�ʼ��Ϊȫ0��������������ø�ֵ    */
{
	int eyeDegree;			/* ���۵������̶�[0,100]��������ƽ��ֵ   */
	int mouthDegree;		/* ��͵��ſ��̶�[0,100]                 */
	int headPosi[3];		/* ��ͷ-תͷ-̧��ͷ[-90,90]  */
	int age;				/* ����[1, 100]      */
	int gender;				/* �Ա�(1=�У�2=Ů  */
	int occlusion;			/* �ڵ�(1=���ڣ�2=��)   */
	int fftSharpness;		/* ������[1, 255]       */
	int hsvSkinRate;		/* ��ɫռ�ȣ�<30Ϊ�Ҷ���    */
	int rotEyeBall;			/* ����ƫת��[0,100]��50����    */
}Face_attr;


Tai_API(int) face_get_attrs(char *fname,Face_attr *p);


/*
���������������жԱ�

����:
	fname �ļ���

���:
	0 ����������
	1 ��������
*/
Tai_API(int) face_exist(char *fname);



struct tagFaceCoord
{//�ܶ�����������������C1(x1, y1), C2(x2, y2)
    int x1;
    int y1;
    int x2;
    int y2;
};

Tai_API(int) face_get_pos(IN char *fname,OUT tagFaceCoord faceArr[15]);








