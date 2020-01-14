cd "${0%/*}"
cd ..
git pull
code Frontend/
(cd Backend/GetOnBoard && dotnet build && dotnet run)
