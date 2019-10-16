CSC  		= csc
PRJC    = ResourceGenerator
_DEFINE = DEBUG

DEFINE  = $(addprefix /define:,$(_DEFINE))
SRC     = $(subst /,\,$(shell gfind src -name \*.cs))


default:
	$(CSC) -out:bin\$(PRJC).debug.exe -target:exe \
		-resource:$(PRJC).resources \
		-win32icon:icon.ico \
		$(DEFINE) \
		$(SRC)


release:
	$(CSC) -out:bin\$(PRJC).exe -target:winexe \
		-resource:$(PRJC).resources \
		-win32icon:icon.ico \
		$(SRC)


run:
	bin\$(PRJC).debug.exe
