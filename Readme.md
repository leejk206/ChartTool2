For Dev

Json 파일 형식이 변경되었습니다.

Normal, Slide, UpFlick, DownFlick의 경우 2개의 int형 변수와 1개의 float형 변수수가 멤버로 존재합니다.
int position : 이전과 같이 라인의 위치를 표시합니다. 1박을 16개의 비트로 쪼개어 정수형으로 나타냈습니다. 즉 노래 시작과 동시에 등장하는 박자는 0, 1박은 16, 2박은 32 등으로 표시됩니다.
int line : 몇 번째 라인에 위치하는지를 나타냅니다. 0~20번째 줄이 있으며 가장 왼쪽부터 0으로 표시됩니다.
float length : 노트의 좌우 길이를 나타냅니다. 기존 비트에 length 만큼 길어지거나 짧아진다고 생각해주시면 되겠습니다.

HoldNote의 경우 4개의 int형 변수와 1개의 float형 변수가 멤버로 존재합니다. (개발 예정)
int position, int line, float length의 경우 다른 노트와 동일합니다.
int noteType : 0, 1, 2로 구분되며 0의 경우 시작, 1의 경우 중간, 2의 경우 끝입니다.
int count : 해당 holdnote가 몇 번째 holdnote인지 표시합니다.

For Design

0. 라인이 총 21개입니다. 7개의 큰 라인이 있고 각 라인 안에 3개의 작은 라인이 존재하여 총 21개입니다.
1. EditMode와 PlayMode의 구분을 삭제했습니다. 음악을 듣는 중에 정지하여 노트를 추가할 수 있습니다.
2. 방향키를 이용해 구간을 이동할 수 있습니다. 좌, 우 방향키의 경우 1초, 상, 하 방향키의 경우 10초를 이동할 수 있습니다.
3. 마우스 스크롤을 이용해 위아래로 이동할 수 있습니다. 다만 이 경우 노래와 싱크가 맞지 않게 되니 사용하지 않는 것을 권장드립니다.
4. save 버튼을 삭제했습니다. 매 노트를 추가하고 삭제할 때마다 자동으로 저장됩니다.
5. load 버튼은 유지하였습니다. 처음 노트를 추가할 때 자동으로 Load되긴 하지만, 수동으로 Load할 수도 있습니다.
6. 노트 추가 버튼을 Q,W,E,R,T로 연동하였습니다. 각각 Normal, Hold, Slide, UpFlick, DownFlick 노트를 추가/삭제하는 버튼입니다.
7. HoldNote는 구현 예정입니다.

06/08 업데이트

For Dev
Json 파일 형식에 float length 멤버를 추가하였습니다. 이 멤버는 노트의 길이 배율을 나타냅니다. 지금 구현하고 계신 노트에 length배만큼 좌우 길이를 곱하면 됩니다.
HoldNote를 구현하였습니다. 다만 count 멤버는 아직 구현하지 못하였습니다. 
FlickNote을 통합하였습니다. int direction 멤버가 추가되었고, direction = 0인 경우 위 플릭 노트를, direction = 1인 경우 아래 플릭 노트를 의미합니다.

For Design
0. Assets - Scenes - SampleScene을 클릭하여 씬을 로드합니다.
1. 노트의 좌, 우 길이를 변경할 수 있습니다. '노트의 원래 위치'에 마우스를 대고 D키를 누르면 우측으로 1칸, A키를 누르면 좌측으로 1칸 줄어듭니다.
2. HoldNote를 구현하였습니다. 키보드 W 키를 이용하여 생성할 수 있고, 각각 시작, 중간, 끝 HoldNote가 있습니다. 
한 줄에서 끝나는 HoldNote는 시작 지점에 HoldNoteStart를 생성하고, 끝 부분에 HoldNoteEnd를 생성하면 됩니다. 여러 줄에 걸친 HoldNote는 시작 지점에 HoldNoteStart를 생성하고, 해당 라인이 끝나는 지점에 HoldNoteMid를 생성합니다.
이후 이어지는 라인의 시작점에 HoldNoteMid를 생성하고, HoldNote가 끝나는 지점에 HoldNoteEnd를 생성하면 됩니다.
W키를 이용해 HoldNote를 생성할 수 있고, 해당 위치에 W키를 한 번 더 누르면 HoldNote의 종류가 바뀝니다.
3. FlickNote는 R키를 이용하여 생성할 수 있습니다. 빨간 색의 위 플릭 노트와 파란 색의 아래 플릭 노트를 토글하여 바꿀 수 있습니다.
4. 곡이 끝나게 되면 BGM을 재생하는 로직이 꼬입니다. 이런 경우 프로그램 자체를 다시 실행해주시길 바랍니다. 빠른 시일 내에 수정하도록 하겠습니다.
5. Hierarchy - Main Camera 클릭 후, Inspector - Camera Mover - BPM에서 BPM을 조정할 수 있습니다.
6. LineSpawner 스크립트의 totalBeats를 수정하면 총 비트 갯수를 바꿀 수 있습니다. 현재는 총 600개의 비트가 설정되어 있습니다.
7. 노트 삭제는 원하는 위치에 다른 종류의 노트 생성 버튼을 누르면 삭제됩니다.