\# Unity RPG 프로젝트



\## 폴더 구조



```

Assets/

&nbsp; ├── Scenes/        # 씬 파일(.unity) 저장

&nbsp; ├── Scripts/       # C# 스크립트

&nbsp; ├── Prefabs/       # 프리팹 에셋

&nbsp; ├── Materials/     # 머티리얼 에셋

&nbsp; ├── Art/           # 2D/3D 아트 리소스

&nbsp; ├── Audio/         # 사운드/음악 파일

&nbsp; ├── Plugins/       # 외부 플러그인

```



\## 설계 의도

\- 유지보수와 협업이 쉬운 구조로 폴더를 분리

\- 아트, 코드, 씬, 프리팹 등 역할별로 명확하게 관리

\- 확장성과 가독성을 고려한 표준 Unity 프로젝트 구조 적용



\## 관리 규칙

\- \*\*Assets/\*\*, \*\*ProjectSettings/\*\*, \*\*Packages/\*\* 폴더는 반드시 버전 관리에 포함

\- \*\*Library/\*\*, \*\*Temp/\*\*, \*\*Build/\*\* 등은 .gitignore로 제외

\- 폴더/파일명은 일관된 네이밍 컨벤션(영문, PascalCase 등) 사용

\- 새 폴더/파일 추가 시 README 또는 Notion에 용도 기록

\- 주요 변경사항은 커밋 메시지에 상세히 남길 것



\## 참고

\- Unity 버전: (예시) 2022.3 LTS

\- .gitignore는 Unity 공식 템플릿 사용

\- 추가 규칙/가이드라인은 Notion 문서 참고



